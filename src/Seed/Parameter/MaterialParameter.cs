using Seed.Parameter.TransitionMatrix;

namespace Seed.Parameter
{
    public class MaterialConfig : IParameter
    {
        public MaterialConfig() { }
        public StructureParameter MaterialStructure { get; set; }
        public TransitionMatrixParameter OperationStructure { get; set; }
    }
}
