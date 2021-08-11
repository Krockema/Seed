using Seed.Parameter.Material;
using System.Collections.Generic;

namespace Seed.Parameter.Operation
{
    public class ResourceGroup : Option<string>
    {
        public ResourceGroup(string name) : base(name) { }
        public AverageDuration OperationDurationAverage { get; set; }
        public VarianceDuration OperationDurationVariance { get; set; }
        public List<ResourceTool> Tools { get; set; }

    }
}
