using MathNet.Numerics.LinearAlgebra;

namespace Seed.Matrix
{
    /// <summary>
    /// Transformes a matrix by accumulating rows acending
    /// </summary>
    public class MatrixProbabilityTransitionInitializer : IMatrixInitializer
    {
        private Matrix<double> Value { get; set; }
        public MatrixProbabilityTransitionInitializer()
        {  }

        public void SetSource(Matrix<double> matrix)
        {
            Value = matrix;
        }

        public double CellValue(int i, int j)
        {
            var accum = 0.0;
            for (int r = 0; r < i; r++)
            {
                accum += Value.At(r, j);
            }
            return accum;
        }
    }
}