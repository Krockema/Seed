namespace Seed.Matrix
{
    public class MatrixNormalInitializer : IMatrixInitializer
    {
        private double Value { get; }
        public MatrixNormalInitializer(int size)
        {
            Value = 1.0 / size;
        }
        /// <summary>
        /// Creates a identity matrix (Einheitsmatrix)
        /// </summary>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <returns></returns>
        public double CellValue(int i, int j)
        {
            return Value;
        }
    }
}