using Seed.Parameter;

namespace Seed.Generator.Material
{
    public interface IWithMaterialConfiguration
    {
        Materials Generate();
        int InitialEdgeCount { get; }
    }
}
