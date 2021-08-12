namespace Seed.Distributions
{
    public class RandomizerCollection: IWithOperationDuration, IWithOperationAmount, IWithTransition
    {
        internal RandomizerCollection() { }
        public IRandomizer OperationDuration { get; internal set; }
        public IRandomizer OperationAmount { get; internal set; }
        public IRandomizer OperationTransition { get; internal set; }
        public static IWithOperationAmount WithTransition(IRandomizer rnd)
        {
            var randomizerCollection = new RandomizerCollection();
            randomizerCollection.OperationTransition = rnd;
            return randomizerCollection;
        }

        public RandomizerCollection Build()
        {
            return this;
        }

        public IWithTransition WithOperationAmount(IRandomizer rnd)
        {
            this.OperationAmount = rnd;
            return this;
        }

        public IWithOperationDuration WithOperationDuration(IRandomizer rnd)
        {
            this.OperationDuration = rnd;
            return this;
        }
    }

    public interface IWithTransition
    {
        RandomizerCollection Build();
    }

    public interface IWithOperationAmount
    {
        IWithOperationDuration WithOperationDuration(IRandomizer rnd);
    }

    public interface IWithOperationDuration
    {
        IWithTransition WithOperationAmount(IRandomizer rnd);
    }
}
