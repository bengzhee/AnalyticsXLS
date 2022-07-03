using QuantLibrary.RatesCurves;
using QuantLibrary;
using ExcelDna.Integration;

namespace AnalyticsDNA
{
    public static class Curves
    {
        [ExcelFunction(Category = "Curves", Description = "Interest rate or discount factor curve")]
        public static string RatesCurve(
            [ExcelArgument(Name = "valuation_date", Description = "Valuation date")] DateTime valuedate,
            [ExcelArgument(Name = "interpolation", Description = "Interpolation method used. Linear only.")] string interpolationmethod,
            [ExcelArgument(Name = "data_type", Description = "Rate or Discount Factor")] string datatype,
            [ExcelArgument(Name = "maturities", Description = "Date array of maturities")] double[] dates,
            [ExcelArgument(Name = "curve_data", Description = "Data array of interest rate or discount factor")] double[] curvedata)
            
        {
            DateTime[] dt_dates = dates.Select(x => DateTime.FromOADate(x)).ToArray();
            RatesCurve ratesCurve = new(valuedate, interpolationmethod, dt_dates, curvedata, datatype);
            Bag.CreateObject(ratesCurve.Id, ratesCurve);
            return ratesCurve.Id.ToString();
        }
    }
}
