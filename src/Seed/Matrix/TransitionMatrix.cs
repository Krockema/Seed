using MathNet.Numerics.LinearAlgebra;
using Seed.Distributions;
using Seed.Parameter.TransitionMatrix;
using System;
using System.Linq;

namespace Seed.Matrix
{
    /// <summary>
    /// Transition Matrix custom initializer.
    /// </summary>
    public class TransitionMatrix
    {
        MatrixBuilder<double> _matrixBuilder = Matrix<double>.Build;
        MatrixProbabilityTransitionInitializer TransitionInitializer;
        Matrix<double> _matrix { get; set; }
        Matrix<double> _probabilityMatrix { get; set; }
        public Matrix<double> GetMatrix => _matrix;

        public void SetMatrix(Matrix<double> matrix)
        {
            _matrix = matrix;
        }
        /// <summary>
        /// Creates a M x M Matrix based on the given Initializer
        /// </summary>
        /// <param name="size"></param>
        /// <param name="initializer"></param>
        public TransitionMatrix(int size, IMatrixInitializer initializer)
        {
            _matrix = _matrixBuilder.Dense(size, size, initializer.CellValue);
            TransitionInitializer = new MatrixProbabilityTransitionInitializer();
        }
        public TransitionMatrix(double[,] matrix)
        {
            _matrix = _matrixBuilder.DenseOfArray(matrix);
            TransitionInitializer = new MatrixProbabilityTransitionInitializer();
        }
        public TransitionMatrix(Matrix<double> matrix)
        {
            _matrix = matrix;
            TransitionInitializer = new MatrixProbabilityTransitionInitializer();
        }
        /// <summary>
        /// Randomizes the matrix by given Random number generator
        /// </summary>
        public void Randomize(IRandomizer rnd)
        {
            _matrix.MapInplace(x => (1.0 + 0.2 * (rnd.Next() - 0.5)) * x, Zeros.Include);
        }

        /// <summary>
        /// Normalizes the rows to [0,1]
        /// </summary>
        public void Normalize()
        {
            foreach (var row in _matrix.EnumerateRowsIndexed())
            {
                var rowSum = row.Item2.Sum();
                row.Item2.MapInplace(x => x / rowSum);
                _matrix.SetRow(row.Item1, row.Item2);
            }
        }
        /// <summary>
        /// Calculate Organizational Degree
        /// </summary>
        /// <returns></returns>
        public double GetOrganizationalDegree()
        {
            var partial = 1.0/(_matrix.ColumnCount -1);
            var og = partial * _matrix.Enumerate().Sum(x => Math.Pow(x - 1.0 / _matrix.ColumnCount, 2));
            return og;                                                                                                                                                                                                   
        }

        public TransitionMatrix HalfAdd(TransitionMatrix other)
        {
            return new TransitionMatrix(_matrix.MapIndexed((i, j, x) => 0.5 * (other.GetMatrix[i, j] + x)));
        }

        public Matrix<double> TransformToProbability()
        {
            TransitionInitializer.SetSource(_matrix);
            _probabilityMatrix = _matrixBuilder.Dense(_matrix.RowCount, _matrix.ColumnCount, TransitionInitializer.CellValue);
            return _probabilityMatrix;
        }
        public int GetJump(int source, double roll)
        {
            return _probabilityMatrix.Row(source).Find(x => x >= roll || x == 1).Item1;
        }

    }
}

