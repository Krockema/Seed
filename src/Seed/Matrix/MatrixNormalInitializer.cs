namespace Seed.Matrix
{
    /// <summary>
    /// Creates a identity matrix (Einheitsmatrix)
    /// </summary>
    public class MatrixNormalInitializer : IMatrixInitializer
    {
        private double Value { get; }
        public MatrixNormalInitializer(int size)
        {
            Value = 1.0 / size;
        }
        public double CellValue(int i, int j)
        {
            return Value;
        }
    }
}