using Seed.Generator.Material;

namespace Seed.Generator.Operation
{
    public interface IWithResourceConfig
    {
        IWithMaterials WithMaterials(IWithOperationsInUse operationsInUse);
    }
}