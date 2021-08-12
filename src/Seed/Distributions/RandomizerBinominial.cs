using System;
using MathNet.Numerics.Distributions;

namespace Seed.Distributions
{
    /// <summary>
    /// Random Number Generator
    /// </summary>
    public class RandomizerBinominial : RandomizerBase
    {
        public RandomizerBinominial(int seed) : base(seed)
        {
        }
        /// <summary>
        /// returns an Integer from Binominial Distribution with Mean and Variance
        /// </summary>
        /// <param name="mean"></param>
        /// <param name="variance"></param>
        /// <returns></returns>
        public override int NextWithMeanAndVariance(double mean, double variance)
        {
            return Binomial.Sample(base.randomSource, 1.0 - variance, (int)(mean / (1.0 - variance)));
        }
    }
}
