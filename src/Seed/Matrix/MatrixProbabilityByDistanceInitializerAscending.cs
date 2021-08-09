using System.Collections;
using System.Linq;
using System.Runtime.Serialization.Formatters;

namespace Seed.Matrix
{       
    /// <summary>
    /// Creates a with probabilities that are decreasing with increasing distance
    /// 
    /// </summary>
    public class MatrixProbabilityByDistanceInitializerAscending : IMatrixInitializer
    {
        private readonly int _Size;
        public MatrixProbabilityByDistanceInitializerAscending(int size)
        {
            _Size = size;
        }

        public double CellValue(int i, int j)
        {
            //if (i == j && i == 1) return 1;
            if (i >= j) return 0;

            var accumulated = 0.0;
            for (int k = i; k <= j; k++)
            {
                accumulated += (2.0 * (k +  1 - i))/((_Size - i) * (_Size - i + 1));
            }
            return accumulated;
        }
    }
}