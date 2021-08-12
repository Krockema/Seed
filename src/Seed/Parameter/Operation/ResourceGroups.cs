using System;
using System.Collections.Generic;

namespace Seed.Parameter.Operation
{
    public class ResourceGroups : Option<string>
    {
        public ResourceGroups() : base ("ResourceConfiguration") {
            DefaultOperationDurationDistributionParameter = new();
            DefaultOperationAmountDistributionParameter = new();
            ResourceGroupList = new();
        }
        public List<ResourceGroup> ResourceGroupList { get; set; }
        public DistributionParameter DefaultOperationDurationDistributionParameter { get; set; }
        public DistributionParameter DefaultOperationAmountDistributionParameter { get; set; }

        public List<ResourceTool> GetToolsFor(int resourceId)
        {
            return this.ResourceGroupList[resourceId].Tools;
        }

        private bool CheckToolValue(int resourceIndex, int toolIndex)
        {
            return this.ResourceGroupList[resourceIndex].Tools[toolIndex].OperationDurationDistributionParameter.Mean != 0;
        }

        private bool CheckResourceValue(int resourceIndex)
        {
            return this.ResourceGroupList[resourceIndex].OperationDurationDistributionParameter.Mean != 0;
        }
        /// <summary>
        /// Returns the Distribution from Config, with fallback > Tools > ResourceGroup > 10
        /// </summary>
        /// <param name="resourceIndex"></param>
        /// <param name="toolIndex"></param>
        /// <returns></returns>
        public TimeSpan GetMeanOperationDurationFor(int resourceIndex, int toolIndex)
        {
            return TimeSpan.FromSeconds(CheckToolValue(resourceIndex, toolIndex) ? 
                                         this.ResourceGroupList[resourceIndex].Tools[toolIndex].OperationDurationDistributionParameter.Mean // Per Tool
                                      : CheckResourceValue(resourceIndex) ? 
                                         this.ResourceGroupList[resourceIndex].OperationDurationDistributionParameter.Mean
                                      : DefaultOperationDurationDistributionParameter.Mean != 0 ?
                                         DefaultOperationDurationDistributionParameter.Mean
                                      : 10);         // Resource Default;
        }
        

        public double GetVarianceOperationDurationFor(int resourceIndex, int toolIndex)
        {
            return CheckToolValue(resourceIndex, toolIndex) ?
                this.ResourceGroupList[resourceIndex].Tools[toolIndex].OperationDurationDistributionParameter.Variance // Per Tool
                : CheckResourceValue(resourceIndex) ? 
                this.ResourceGroupList[resourceIndex].OperationDurationDistributionParameter.Variance
                : DefaultOperationDurationDistributionParameter.Variance;    // No further check needed as int is 0 anyway
        }

        public ResourceGroups WithDefaultOperationsDurationVariance(double varianceInPercent)
        {
            this.DefaultOperationDurationDistributionParameter.Variance = varianceInPercent;
            return this;
        }
        public ResourceGroups WithDefaultOperationsDurationMean(TimeSpan mean)
        {
            this.DefaultOperationDurationDistributionParameter.Mean = mean.TotalSeconds;
            return this;
        }
        public ResourceGroups WithDefaultOperationsAmountVariance(double varianceInPercent)
        {
            this.DefaultOperationAmountDistributionParameter.Variance = varianceInPercent;
            return this;
        }
        public ResourceGroups WithDefaultOperationsAmountMean(int quantity)
        {
            this.DefaultOperationAmountDistributionParameter.Mean = quantity;
            return this;
        }
        public ResourceGroups WithResourceGroup(List<ResourceGroup> groups)
        {
            this.ResourceGroupList.AddRange(groups); 
            return this;
        }
    }
}
