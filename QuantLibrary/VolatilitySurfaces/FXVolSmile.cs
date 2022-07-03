using QuantLibrary.RatesCurves; 

namespace QuantLibrary.VolatilitySurfaces
{
    internal class FXVolSmile
    {
        public FXVolSmile(
            DateTime valueDate, 
            double forward, 
            DateTime forwardDate, 
            double[] smileData, 
            DateTime expiryDate, 
            string[] deltas, 
            string deltaType, 
            RatesCurve domesticCurve, 
            RatesCurve foreignCurve, strikeInterpolationMethod, strikeExtrapolationMethod)
    }
}
