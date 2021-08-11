using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Seed.Parameter.Operation
{
    public class ResourceGroups : Option<List<ResourceGroup>>
    {
        public ResourceGroups(List<ResourceGroup> groups) : base(groups) { }

        public List<ResourceTool> GetToolsFor(int resourceId)
        {
            return this.Value[resourceId].Tools;
        }

        public TimeSpan GetAverageOperationDurationFor(int resourceIndex, int toolIndex)
        {
            return (this.Value[resourceIndex].Tools[toolIndex].OperationDurationAverage // Per Tool
                    ?? this.Value[resourceIndex].OperationDurationAverage)              // Resource Default
                    .Value;                                                             // Value 
        }
        public double GetVarianceOperationDurationFor(int resourceIndex, int toolIndex)
        {
            return (this.Value[resourceIndex].Tools[toolIndex].OperationDurationVariance // Per Tool
                    ?? this.Value[resourceIndex].OperationDurationVariance)              // Resource Default
                    .Value;                                                             // Value 
        }
    }
}
