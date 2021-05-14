using System;
using System.Collections.Generic;
using System.Text;

namespace Seed.Distributions
{
    /// <summary>
    /// Random Number Generator
    /// </summary>
    public class Randomizer : IRandomizer
    {
        private Random random;

        public Randomizer(int seed)
        {
            random = new Random(seed);
        }
        /// <summary>
        /// returns a double in range [0.0, 1.0]
        /// </summary>
        /// <returns></returns>
        public double Next()
        {
            return random.NextDouble();
        }

        public int Next(int max)
        {
            return random.Next(max);
        }

    }
}
