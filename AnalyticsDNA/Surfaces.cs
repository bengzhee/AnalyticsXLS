using QuantLibrary.VolatilitySurfaces;
using ExcelDna.Integration;

namespace AnalyticsDNA
{
    public static class Surfaces
    {
        [ExcelFunction(Category = "Surfaces", Description = "Get the SABR volatility for given smile")]
        public static double SABRVolatility(
            [ExcelArgument(Name = "forward", Description = "ATM forward")] double forward,
            [ExcelArgument(Name = "strike", Description = "Strike")] double strike,
            [ExcelArgument(Name = "time_to_maturity", Description = "Time to Maturity")] double ttm,
            [ExcelArgument(Name = "atm_volatility", Description = "At the money volatility")] double atmVol,
            [ExcelArgument(Name = "beta", Description = "SABR beta")] double beta,
            [ExcelArgument(Name = "rho", Description = "SABR rho")] double rho,
            [ExcelArgument(Name = "nu", Description = "SABR nu")] double nu)
        {
            return SABR.SabrVol(forward, strike, ttm, atmVol, beta, rho, nu);
        }
        
        [ExcelFunction(Category = "Surfaces", Description = "Solve for Rho and Nu given a surface and Beta")]
        public static double[] SABRFitRhoNuGivenBeta(
            [ExcelArgument(Name = "forward", Description = "ATM forward")] double forward,
            [ExcelArgument(Name = "time_to_maturity", Description = "Time to Maturity")] double ttm,
            [ExcelArgument(Name = "atm_volatility", Description = "At the money volatility")] double atmVol,
            [ExcelArgument(Name = "beta", Description = "SABR beta")] double beta,
            [ExcelArgument(Name = "strikes", Description = "Array of strikes")] double[] strikes,
            [ExcelArgument(Name = "volatilities", Description = "Array of volatilities")] double[] volatilities)
        {
            return SABR.FitRhoAndNuFixedBeta(forward, ttm, atmVol, beta, strikes, volatilities);
        }

    }
}
