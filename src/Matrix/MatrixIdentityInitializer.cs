namespace Seed.Matrix
{
    public class MatrixIdentityInitializer : IMatrixInitializer
    {
        /// <summary>
        /// Creates a identity matrix (Einheitsmatrix)
        /// </summary>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <returns></returns>
        public double CellValue(int i, int j)
        {
            return (i == j) ? 1.0 : 0.0;
        }
    }
}