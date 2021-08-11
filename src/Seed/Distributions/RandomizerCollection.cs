namespace Seed.Distributions
{
    public class RandomizerCollection
    {
        public IRandomizer OperationDuration { get; }
        public IRandomizer OperationAmount { get; }
        public IRandomizer OperationTransition { get; }
    }
}
