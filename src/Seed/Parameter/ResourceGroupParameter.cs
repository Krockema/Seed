using Seed.Parameter.Operation;

namespace Seed.Parameter
{
    public class ResourceGroupParameter : IParameter
    {
        public ResourceGroupParameter() 
        {
            DefaultOperationDurationDistributionParameter = new();
            DefaultOperationAmountDistributionParameter = new();
            ResourceGroupList = new();
        }
        public List<ResourceGroup> ResourceGroupList { get; set; }
        public DistributionParameter DefaultOperationDurationDistributionParameter { get; set; }
        public DistributionParameter DefaultOperationAmountDistributionParameter { get; set; }
        public double DefaultCostRateIdleTime { get; set; }
        public double DefaultCostRateProcessing { get; set; }
        public double DefaultCostRateSetup { get; set; }
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

        public double GetCostRateIdleTimeFor(int resourceIndex)
        {
            return this.ResourceGroupList[resourceIndex].CostRateIdleTime != 0.0 ? 
                   /* then */ this.ResourceGroupList[resourceIndex].CostRateIdleTime 
                   /* else */ : this.DefaultCostRateIdleTime;
        }

        public double GetCostRateSetupFor(int resourceIndex)
        {
            return this.ResourceGroupList[resourceIndex].CostRateSetup != 0.0 ?
                   /* then */ this.ResourceGroupList[resourceIndex].CostRateSetup
                   /* else */ : this.DefaultCostRateSetup;
        }

        public double GetCostRateProcessingFor(int resourceIndex)
        {
            return this.ResourceGroupList[resourceIndex].CostRateProcessing != 0.0 ?
                   /* then */ this.ResourceGroupList[resourceIndex].CostRateProcessing
                   /* else */ : this.DefaultCostRateProcessing;
        }

        public double GetVarianceOperationDurationFor(int resourceIndex, int toolIndex)
        {
            return CheckToolValue(resourceIndex, toolIndex) ?
                this.ResourceGroupList[resourceIndex].Tools[toolIndex].OperationDurationDistributionParameter.Variance // Per Tool
                : CheckResourceValue(resourceIndex) ? 
                this.ResourceGroupList[resourceIndex].OperationDurationDistributionParameter.Variance
                : DefaultOperationDurationDistributionParameter.Variance;    // No further check needed as int is 0 anyway
        }

        public ResourceGroupParameter WithDefaultOperationsDurationVariance(double varianceInPercent)
        {
            this.DefaultOperationDurationDistributionParameter.Variance = varianceInPercent;
            return this;
        }
        public ResourceGroupParameter WithDefaultOperationsDurationMean(TimeSpan mean)
        {
            this.DefaultOperationDurationDistributionParameter.Mean = mean.TotalSeconds;
            return this;
        }
        public ResourceGroupParameter WithDefaultOperationsAmountVariance(double varianceInPercent)
        {
            this.DefaultOperationAmountDistributionParameter.Variance = varianceInPercent;
            return this;
        }
        public ResourceGroupParameter WithDefaultOperationsAmountMean(int quantity)
        {
            this.DefaultOperationAmountDistributionParameter.Mean = quantity;
            return this;
        }
        public ResourceGroupParameter WithResourceGroup(List<ResourceGroup> groups)
        {
            this.ResourceGroupList.AddRange(groups); 
            return this;
        }
    }
}
