using System.Drawing;
using System.Runtime.Serialization.Formatters;

namespace Seed.Matrix
{       
    /// <summary>
    /// Creates a with probabilities that are decreasing with increasing distance
    /// 
    /// </summary>
    public class MatrixProbabilityByDistanceInitializerDescendingMirror : IMatrixInitializer
    {
        public readonly int _Size;
        public MatrixProbabilityByDistanceInitializerDescendingMirror(int size)
{
            _Size = size;
        }

        public double CellValue(int i, int j)
        {
            
            if (i == j) return 0;
            if (j <= i) return 0; 
            if (i == 1 && j == 0) return 1;
            if (j <= 1) return 0;
            j++;
            var accumulated = 0.0;
            for (int k = j; k < i; k++)
            {
                accumulated += (2.0*k) / (i * (i - 1));
            }         
            return accumulated;
        }
    }
}
