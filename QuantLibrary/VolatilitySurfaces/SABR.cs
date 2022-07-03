using QuantLibrary.Maths;

namespace QuantLibrary.VolatilitySurfaces
{
    public static class SABR
    {
        public static double SabrVol(double forward, double strike, double timetoMaturity, double atmVol, double beta, double rho, double nu)
        {
            double alpha = AlphaFromATM(atmVol, forward, timetoMaturity, beta, rho, nu);
            double oneMinusBeta = 1 - beta;
            double z = nu / alpha * Math.Pow((forward * strike), oneMinusBeta / 2) * Math.Log(forward / strike);
            double chi = Math.Log((Math.Sqrt(1 - 2 * rho * z + z * z) + z - rho) / (1 - rho));
            double fk_aux = Math.Pow(forward * strike, oneMinusBeta / 2);

            // Hagan 2.17a
            double aux1 = oneMinusBeta * Math.Log(forward * strike);
            double term1 = alpha / (fk_aux * (1 + Math.Pow(aux1, 2) / 24 + Math.Pow(aux1, 4) / 1920));
            double term2 = z / chi;
            double term3 = 1 + timetoMaturity * (Math.Pow(oneMinusBeta * alpha / fk_aux, 2) / 24
                           + rho * beta * nu * alpha / fk_aux / 4
                           + (2 - 3 * rho * rho) * nu * nu / 24);

            return term1 * term2 * term3;
        }

        public static double[] FitRhoAndNuFixedBeta(double forward, double timeToMaturity, double atmVol, double beta, double[] strikes, double[] volatilities)
        {
            double[] parameters = new double[] { 0.0, 0.3 };
            double[] s = new double[] { 1, 1 };
            double[] bndl = new double[] { -1, 0 };
            double[] bndu = new double[] { 1, 9999 };
            double epsx = 0.000000001;
            int maxits = 10000;
            alglib.minlmstate state;
            alglib.minlmreport report;
            alglib.minlmcreatev(volatilities.Length, parameters, 0.00001, out state);
            alglib.minlmsetbc(state, bndl, bndu);
            alglib.minlmsetcond(state, epsx, maxits);
            alglib.minlmsetscale(state, s);
            
            void func1(double[]p, double[] fi, object obj)
            {
                double alpha = AlphaFromATM(atmVol, forward, timeToMaturity, beta, p[0], p[1]);
                for (int i = 0; i < volatilities.Length; i++)
                {
                    fi[i] = SabrVol(forward, strikes[i], timeToMaturity, atmVol, beta, p[0], p[1]) - volatilities[i];
                }

            }    
            
            alglib.minlmoptimize(state, func1, null, null);
            alglib.minlmresults(state, out parameters, out report);

            return parameters;
        }
        public static double AlphaFromATM(double atmVol, double forward, double timeToMaturity, double beta, double rho, double nu)
        {
            double oneMinusBeta = 1 - beta;
            double forwardToPowerOneMinusBeta = System.Math.Pow(forward, oneMinusBeta);
            double a3 = oneMinusBeta * oneMinusBeta / 24 / forwardToPowerOneMinusBeta / forwardToPowerOneMinusBeta * timeToMaturity;
            double a2 = 0.25 * rho * beta * nu / forwardToPowerOneMinusBeta * timeToMaturity;
            double a1 = 1 + (2 - 3 * rho * rho) / 24 * nu * nu * timeToMaturity;
            double a0 = -atmVol * forwardToPowerOneMinusBeta;
            if (System.Math.Abs(a3) <= MathConstants.DBL_EPS)
            {
                if (System.Math.Abs(a2) <= MathConstants.DBL_EPS)
                {
                    return -a0 / a1;
                }
                double D = a1 * a1 - 4 * a2 * a0;
                if (D < 0)
                    return -0.5 * a1 / a2;
                double aux = System.Math.Sqrt(D);
                double y1 = 0.5 * (-a1 - aux) / a2;
                double y2 = 0.5 * (-a1 + aux) / a2;
                if (y1 > 0)
                    return y1;
                return y2;
            }
            bool isComplex = false;
            double x1 = 0, x2 = 0, x3 = 0;
            Cardano.Solve(a2 / a3, a1 / a3, a0 / a3, ref x1, ref x2, ref x3, ref isComplex);

            double result = double.MaxValue;
            if (isComplex)
            {
                if (x1 > 0)
                    result = x1;
            }
            else
            {
                if (x1 > 0 && x1 < result)
                    result = x1;
                if (x2 > 0 && x2 < result)
                    result = x2;
                if (x3 > 0 && x3 < result)
                    result = x3;
            }
            return result;
        }
    }
}
