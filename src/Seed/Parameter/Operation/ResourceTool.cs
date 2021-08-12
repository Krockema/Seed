using Seed.Parameter.Material;
using System;

namespace Seed.Parameter.Operation
{
    public class ResourceTool
    {
        public ResourceTool(string name) 
        {
            Name = name;
            SetupDistributionParameter = new();
            OperationDurationDistributionParameter = new();
        }
        public string Name { get; set; }

        public DistributionParameter SetupDistributionParameter { get; set; }
        public DistributionParameter OperationDurationDistributionParameter { get; set; }

        public ResourceTool WithOperationDurationAverage(TimeSpan timeSpan)
        {
            this.OperationDurationDistributionParameter.Mean = timeSpan.TotalSeconds;
            return this;
        }
        public ResourceTool WithOperationDurationVariance(double varianceInPercent)
        {
            this.OperationDurationDistributionParameter.Variance = varianceInPercent;
            return this;
        }
        public ResourceTool WithSetupDurationAverage(TimeSpan timeSpan)
        {
            this.SetupDistributionParameter.Mean = timeSpan.TotalSeconds;
            return this;
        }
        public ResourceTool WithSetupDurationVariance(double varianceInPercent)
        {
            this.SetupDistributionParameter.Variance = varianceInPercent;
            return this;
        }

    }
}
