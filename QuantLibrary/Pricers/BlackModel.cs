using QuantLibrary.Maths;

namespace QuantLibrary.Pricers
{
    public sealed class BlackModel 
    {
        public BlackModel(double forward, double strike, double maturity, double rate, double volatility, bool iscall)
        {
            this.Forward = forward;
            this.Strike = strike;
            this.Maturity = maturity;
            this.Rate = rate;
            this.Volatility = volatility;
            this.IsCall = iscall;
            
            TimeVol = Volatility * Math.Sqrt(Maturity);
            d1 = Math.Log(Forward / Strike) / TimeVol + 0.5 * TimeVol;
            d2 = d1 - TimeVol;

            if (IsCall)
            {
                phi = 1;
            }
            else
            {
                phi = -1;
            }

        }
        
        public double Calculate(string RetVal)
        {
            switch (RetVal)
            {
                case "Price":
                    return Price();

                case "Delta":
                    return Delta();

                case "Gamma":
                    return Gamma();

                case "Vega":
                    return Vega();

                case "Theta":

                    return Theta();

                default:
                    throw new Exception(String.Format("Return Value {0} is not a possible option", RetVal));
            }
        }

        public double Price()
        {
            return Math.Exp(-Rate * Maturity) * (phi * Forward * Gaussian.CDF(phi * d1) - phi * Strike * Gaussian.CDF(phi * d2));
        }

        public double Delta()
        {
            return phi * Math.Exp(-Rate * Maturity) * Gaussian.CDF(phi * d1);
        }

        public double Gamma()
        { 
            return Math.Exp(-Rate * Maturity) / (Forward * TimeVol) * Gaussian.PDF(d1); 
        }

        public double Vega()
        {
            return Math.Exp(-Rate * Maturity) * Forward * Math.Sqrt(Maturity) * Gaussian.PDF(d1);
        }

        public double Theta() 
        {
            return Forward * Gaussian.PDF(d1) * Volatility / (2 * Math.Sqrt(Maturity)) - (Rate * Math.Exp(-Rate * Maturity)) * (Forward * Gaussian.CDF(d1) + Strike * Gaussian.CDF(d2));
        }

        public double Forward { get; set; }
        public double Strike { get; set; }
        public double Maturity { get; set; }
        public double Rate { get; set; }
        public double Volatility { get; set; }
        public bool IsCall { get; set; }
        private double phi { get; set; }
        private double d1 { get; set; }
        private double d2 { get; set; }
        private double TimeVol { get; set; }

    }
}
