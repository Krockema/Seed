using Seed.Parameter.TransitionMatrix;

namespace Seed.Parameter
{
    public class MaterialConfig : IParameter
    {
        public MaterialConfig() {
            StructureParameter = new();
            TransitionMatrixParameter = new ();
        }
        public StructureParameter StructureParameter { get; set; }
        public TransitionMatrixParameter TransitionMatrixParameter { get; set; }
    }
}
