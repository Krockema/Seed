using System;
using MathNet.Numerics.Distributions;

namespace Seed.Distributions
{
    /// <summary>
    /// Random Number Generator
    /// </summary>
    public class RandomizerLogNormal : RandomizerBase
    {
        public RandomizerLogNormal(int seed) : base(seed)
        {
        }

        /// <summary>
        /// returns an Integer from Normal Distribution with Mean and Variance
        /// </summary>
        /// <param name="mean"></param>
        /// <param name="variance"></param>
        /// <returns></returns>
        public override int NextWithMeanAndVariance(double mean, double variance)
        {
            return Convert.ToInt32(LogNormal.WithMeanVariance(mean, variance, randomSource).Sample());
        }
    }
}
