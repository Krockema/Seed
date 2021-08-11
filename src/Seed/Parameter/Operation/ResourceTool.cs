using Seed.Parameter.Material;

namespace Seed.Parameter.Operation
{
    public class ResourceTool : Option<string>
    {
        public ResourceTool(string name) : base(name) { }
        public AverageDuration SetupDurationAverage { get; set; }
        public AverageDuration OperationDurationAverage { get; set; }
        public VarianceDuration OperationDurationVariance { get; set; }
    }
}
