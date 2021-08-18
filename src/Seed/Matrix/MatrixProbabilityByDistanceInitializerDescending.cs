namespace Seed.Matrix
{       
    /// <summary>
    /// Creates a with probabilities that are decreasing with increasing distance
    /// 
    /// </summary>
    public class MatrixProbabilityByDistanceInitializerDescending : IMatrixInitializer
    {
        public MatrixProbabilityByDistanceInitializerDescending()
        {
        }

        public double CellValue(int i, int j)
        {
            
            if (i == j) return 0;
            if (j >= i) return 0; 
            if (i == 1 && j == 0) return 1;
            if (i <= 1) return 0;
            i++;
            var accumulated = 0.0;
            for (int k = 1; k <= j+1; k++)
            {
                accumulated += (2.0*k) / (i * (i - 1));
            }         
            return accumulated;
        }
    }
}
