using MathNet.Numerics.LinearAlgebra;
using Seed.Parameter.TransitionMatrix;

namespace Seed.Matrix
{
    public class ProbabilityByDistanceMatrix
    {
        private MatrixBuilder<double> _matrixBuilder = Matrix<double>.Build;
        Matrix<double> _matrix { get; set; }
        public Matrix<double> GetMatrix => _matrix;

        /// <summary>
        /// Creates a M x M Matrix based on the given Initializer
        /// </summary>
        /// <param name="size"></param>
        /// <param name="initializer"></param>
        public ProbabilityByDistanceMatrix(int size, IMatrixInitializer initializer)
        {
            _matrix = _matrixBuilder.Dense(size, size, initializer.CellValue);
        }

        public Vector<double> GetValuesFor(int rowIndex)
        {
            return _matrix.Row(rowIndex);
        }


        public int GetRowJump(int rowLevel, double roll)
        {
            return _matrix.Row(rowLevel).Find(x => x >= roll || x == 1).Item1;
        }
    }
}