using Seed.Parameter.Material;
using System;
using System.Collections.Generic;

namespace Seed.Parameter.Operation
{
    public class ResourceGroup
    {
        public ResourceGroup(string name) 
        {
            Name = name;
            OperationDurationDistributionParameter = new();
        }

        public string Name { get; set; }
        public long ResourceQuantity { get; set; }
        public DistributionParameter OperationDurationDistributionParameter { get; set; }
        public List<ResourceTool> Tools { get; set; }

        public ResourceGroup WithDefaultDurationMean(TimeSpan timeSpan)
        {
            this.OperationDurationDistributionParameter.Mean = timeSpan.TotalSeconds;
            return this;
        }
        public ResourceGroup WithDefaultDurationVariance(double varianceInPercent)
        {
            this.OperationDurationDistributionParameter.Variance = varianceInPercent;
            return this;
        }
        public ResourceGroup WithTools(List<ResourceTool> tools)
        {
            this.Tools = tools;
            return this;
        }
        public ResourceGroup WithResourceuQuantity(int quantity)
        {
            this.ResourceQuantity = quantity;
            return this;
        }
    }
}
