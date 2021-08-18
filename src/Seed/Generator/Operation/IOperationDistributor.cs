using Seed.Data;

namespace Seed.Generator.Operation
{
    public interface IOperationDistributor
    {
        MaterialNodeOperation[] GenerateOperationsFor(MaterialNode node);
        double[,] TargetTransitions { get; }
    }
}