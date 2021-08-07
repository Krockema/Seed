namespace Seed.Matrix
{       
    /// <summary>
    /// Creates a identity matrix (Einheitsmatrix)
    /// </summary>
    public class MatrixIdentityInitializer : IMatrixInitializer
    {
        public double CellValue(int i, int j)
        {
            return (i == j) ? 1.0 : 0.0;
        }
    }
}