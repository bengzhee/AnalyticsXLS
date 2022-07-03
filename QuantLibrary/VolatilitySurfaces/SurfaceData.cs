using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuantLibrary.VolatilitySurfaces
{
    sealed public class SurfaceData
    {
        public SurfaceData(DateTime valuationdate, string[] expiries, string[] moneyness, double[,] voldata)
        {
            this.ValueDate = valuationdate;
            this.Expiries = expiries;
            this.Moneyness = moneyness;
            this.VolatilityData = voldata;

            DataRows = VolatilityData.GetLength(0);
            DataColumns = VolatilityData.GetLength(1);
            SanityChecks();
        }

        public void SanityChecks()
        {
            for (int r = 0; r < DataRows; r++)
            {
                for (int c = 0; c < DataColumns; c++)
                {
                    if (VolatilityData[r, c] <= 0.0)
                    {
                        throw new Exception("Volatility data contains negative values");
                    }
                }
            }
            if (DataRows != Expiries.Length)
            {
                throw new Exception("Length of expiries do not match number of data rows");
            }
            if (DataColumns != Moneyness.Length)
            {
                throw new Exception("Length of deltas do not match number of data columns");
            }
        }
        public DateTime ValueDate { get; set; }
        public string[] Expiries { get; set; }
        public string[] Moneyness { get; set; }
        public double[,] VolatilityData { get; set; }
        public int DataRows { get; set; }
        public int DataColumns { get; set; }
    }
}
