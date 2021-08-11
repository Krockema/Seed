using System;
using MathNet.Numerics.Distributions;
using MathNet.Numerics.Random;

namespace Seed.Distributions
{
    /// <summary>
    /// Random Number Generator
    /// </summary>
    public class Randomizer : IRandomizer
    {
        private Random randomSource;

        public Randomizer(int seed)
        {
            randomSource = new Random(seed);
        }
        /// <summary>
        /// returns a double in range [0.0, <1.0]
        /// </summary>
        /// <returns></returns>
        public double Next()
{
            return ContinuousUniform.Sample(randomSource, 0, 1);
        }

        /// <summary>
        /// returns a integer in range [0.0, <maxExclusive]
        /// </summary>
        /// <param name="max">Exclusive</param>
        /// <returns></returns>
        public int Next(int maxExclusive)
        {
            return DiscreteUniform.Sample(randomSource, 0, maxExclusive - 1);
        }

        public int NextWithMeanAndVariance(double mean, double variance)
        {
            return Convert.ToInt32(Normal.WithMeanVariance(mean, variance, randomSource).Sample());
        }
    }
}
