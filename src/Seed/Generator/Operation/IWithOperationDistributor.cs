using Seed.Data;

namespace Seed.Generator.Operation
{
    public interface IWithOperationDistributor
    {
        IOperationGenerator WithMaterials(MaterialNode[] materialsWithoutPurchase);
    }
}
