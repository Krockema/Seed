namespace Seed.Parameter
{
    public class MaterialStructureParameter : IParameter
    {
        public MaterialStructureParameter() { }
        public double ComplexityRatio {  get; set; }
        public double ReuseRatio {  get; set; }
        public int NumberOfSalesMaterials {  get; set; }
        public int VerticalIntegration {  get; set; }

    }
}
