
namespace QuantLibrary.RatesCurves
{
    public sealed class RatesCurve
    {

        public RatesCurve(DateTime valueDate, string interpolationMethod, DateTime[] dates, double[] curveData, string dataType)
        {
            Id = Guid.NewGuid();
            ValueDate = valueDate;
            InterpolationMethod = interpolationMethod;
            Dates = dates;

            Abscissas = new double[Dates.Length];
            for (int iDate = 0; iDate < dates.Length; iDate++)
            {
                Abscissas[iDate] = (Dates[iDate] - ValueDate).Days / 365.0;
            }
            
            if (dataType.ToLower() == "discount factors")
            {
                RatesData = new double[curveData.Length];
                for (int i = 0; i < curveData.Length; i++)
                {
                    RatesData[i] = (-Math.Log(curveData[i]) / Abscissas[i]);
                }
                
            }
            else if (dataType.ToLower() == "rates")
            {
                RatesData = curveData;
            }
            else 
            {
                throw new Exception(String.Format("Data Type {0} is not supported. Possible: [discount factors, rates]", dataType));
            }
        }

        public double DF(DateTime date)
        {
            double period = (date - ValueDate).Days / 365.0;
            return this.DF(period);
        }
            
        private double DF(double period)
        {
            if (period < 0) 
                throw new ArgumentException(String.Format("Date cannot be earlier than valuation date ({0})", ValueDate));
            if (period < Abscissas[0])
                return Math.Exp(-RatesData[0] * period);
            if (period > Abscissas[^1])
                return Math.Exp(-RatesData[Abscissas.Length - 1] * period);

            double xl, yl, xr, yr;
            int index = Array.FindIndex(Abscissas, x => x >= period);
            double interpolatedValue;

            if (Abscissas.Contains(period))
            {
                interpolatedValue = RatesData[index];
            }
            else
            {
                xl = Abscissas[index - 1];
                xr = Abscissas[index];
                yl = RatesData[index - 1];
                yr = RatesData[index];
                // currently we support only linear interpolation
                interpolatedValue = (xr - period) / (xr - xl) * yl + (period - xl) / (xr - xl) * yr;

            }
            
            return Math.Exp(-interpolatedValue * period);
        }

        public Guid Id { get; set; }
        public DateTime ValueDate { get; set; }
        public string InterpolationMethod { get; set; }
        public DateTime[] Dates { get; set; }
        public double[] RatesData { get; set; }
        public double[] Abscissas { get; set; }  // abscissas are the period in years for each of the date in Dates array i.e. convert datetime to double
        enum InterpMethods
        {
            Linear
        }        
    }
}
