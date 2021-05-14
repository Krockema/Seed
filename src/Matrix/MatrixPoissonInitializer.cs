using System;
using MathNet.Numerics;
using Seed.Parameter.TransitionMatrix;

namespace Seed.Matrix
{
    public class MatrixPoissonInitializer : IMatrixInitializer
    {
        private Lambda Lambda { get; set; }

        public MatrixPoissonInitializer(Lambda lambda)
        {
            Lambda = lambda;
        }

        /// <summary>
        /// initialize the matrix with custom Poisson distribution
        /// </summary>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <param name="lambda"></param>
        /// <returns></returns>
        public double CellValue(int i, int j)
        {
            if (i <= j)
            {
                return Math.Pow(Lambda.Value, j - i) / SpecialFunctions.Factorial(j - i);
            }
            else
            {
                return Math.Pow(Lambda.Value, i - j) / (2 * SpecialFunctions.Factorial(i - j));
            }
        }

    }
}