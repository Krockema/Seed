using Seed.Data;

namespace Seed.Generator
{
    public interface IOperationDistributor
    {
        MaterialNodeOperation[] GenerateOperationsFor(MaterialNode node);
    }
}