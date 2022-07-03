using QuantLibrary.Pricers;
using ExcelDna.Integration;

// make return value take in multiple values
namespace AnalyticsDNA
{
    public static class Pricings
    {
        [ExcelFunction(Category = "Pricers", Description = "Price of a european option in the Black model")]
        
        public static double BlackEUOption(
            [ExcelArgument(Name = "forward", Description = "Forward rate")] double forward,
            [ExcelArgument(Name = "strike", Description = "Strike rate")] double strike,
            [ExcelArgument(Name = "time_to_maturity", Description = "Time to maturity in years")] double maturity,
            [ExcelArgument(Name = "interest_rate", Description = "Interest rate over option period")] double rate,
            [ExcelArgument(Name = "volatility", Description = "Volatility")] double volatility,
            [ExcelArgument(Name = "is_call", Description = "Product is a call indicator")] bool iscall,
            [ExcelArgument(Name = "return", Description = "Return: price, delta, gamme, vega")] string retval)

        {
            BlackModel blackModel = new(forward, strike, maturity, rate, volatility, iscall);
            return blackModel.Calculate(retval);
        }

    }
}