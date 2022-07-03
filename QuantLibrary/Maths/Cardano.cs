
namespace QuantLibrary.Maths
{
    public static class Cardano
    {
        // Finds roots of the cubic equation
        //  3     2
        // x + b x + c x + d = 0
        public static void Solve(double b, double c, double d, 
            ref double x1, ref double x2, ref double x3, ref bool isComplex)
        {
            double a = 1;
            double p = (3 * a * c - b * b) / (3 * a * a);
            double q = (2 * b * b * b - 9 * a * b * c + 27 * a * a * d) / (27 * a * a * a);
            double t1, t2, t3;
            double aux = 3 * q / (2 * p);
            if (p > 0)
            {
                t1 = -2 * System.Math.Sqrt(p / 3) * System.Math.Sinh(1 / 3.0 * Asinh(aux * System.Math.Sqrt(3 / p)));
                t2 = 0;
                t3 = 0;
                isComplex = true;
            }
            else if (p < 0)
            {
                double aux2 = System.Math.Sqrt(-p / 3);
                double aux3 = 2 * MathConstants.PI / 3;
                if (4 * p * p * p + 27 * q * q <= 0)
                {
                    t1 = 2 * aux2 * System.Math.Cos(1 / 3.0 * System.Math.Acos(aux / aux2) - 0 * aux3);
                    t2 = 2 * aux2 * System.Math.Cos(1 / 3.0 * System.Math.Acos(aux / aux2) - 1 * aux3);
                    t3 = 2 * aux2 * System.Math.Cos(1 / 3.0 * System.Math.Acos(aux / aux2) - 2 * aux3);
                    isComplex = false;
                }
                else
                {
                    t1 = -2 * System.Math.Abs(q) / q * System.Math.Sqrt(-p / q) * System.Math.Cosh(1 / 3.0 * Acosh(-3 
                        * System.Math.Abs(q) / (2 * p) * System.Math.Sqrt(-3 / p)));
                    t2 = 0;
                    t3 = 0;
                }
            }
            else
            {
                t1 = System.Math.Pow(-q, 1 / 3.0);
                t2 = 0;
                t3 = 0;
                isComplex = true;
            }
            x1 = t1 - b / 3;
            if (!isComplex)
            {
                x2 = t2 - b / 3;
                x3 = t3 - b / 3;
            }
        }
        private static double Asinh(double z)
        {
            return System.Math.Log(z + System.Math.Sqrt(1 + z * z));
        }
        private static double Acosh(double z)
        {
            return System.Math.Log(z + System.Math.Sqrt(z + 1) * System.Math.Sqrt(z- 1));
        }
    }
}
