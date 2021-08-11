namespace Seed.Distributions
{
    public interface IRandomizer
    {
        /// <summary>
        /// returns a double in range [0.0, <1.0]
        /// </summary>
        /// <returns></returns>
        double Next();
        /// <summary>
        /// returns a integer in range [0.0, <max]
        /// </summary>
        /// <param name="max"></param>
        /// <returns></returns>
        int Next(int max);
        /// <summary>
        /// returns a integer based on given mean and variance
        /// </summary>
        /// <param name="mean"></param>
        /// <param name="variance"></param>
        /// <returns>integer</returns>
        int NextWithMeanAndVariance(double mean, double variance);
    }
}