namespace Seed.Distributions
{
    public interface IRandomizer
    {
        double Next();
        int Next(int max);
    }
}