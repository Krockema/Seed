using System;
using MathNet.Numerics;
using Seed.Parameter.TransitionMatrix;

namespace Seed.Matrix
{
    /// <summary>
    /// initialize the matrix with custom Poisson distribution
    /// </summary>
    public class MatrixPoissonInitializer : IMatrixInitializer
    {
        private double Lambda { get; set; }

        public MatrixPoissonInitializer(double lambda)
        {
            Lambda = lambda;
        }

        public double CellValue(int i, int j)
        {
            if (i <= j)
            {
                return Math.Pow(Lambda, j - i) / SpecialFunctions.Factorial(j - i);
            }
            else
            {
                return Math.Pow(Lambda, i - j) / (2 * SpecialFunctions.Factorial(i - j));
            }
        }

    }
}