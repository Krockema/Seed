using Seed.Distributions;

namespace Seed.Generator.Operation
{
    public interface IWithTransitionMatrix
    {
        IWithRandomizerCollection WithRandomizerCollection(RandomizerCollection randomizerCollection);
    }
}
