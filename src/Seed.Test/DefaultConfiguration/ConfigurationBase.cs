using Seed.Parameter.Operation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Seed.Test.DefaultConfiguration
{
    public class ConfigurationBase
    {
        public static ResourceGroups CreateResourceGroups()
        {
            var rsSaw = new ResourceGroup("Saw")
                .WithResourceuQuantity(2)
                .WithTools(new List<ResourceTool> {
                    new ResourceTool("Blade 4mm").WithOperationDurationAverage(TimeSpan.FromMinutes(6)).WithOperationDurationVariance(0.20),
                    new ResourceTool("Blade 6mm").WithOperationDurationAverage(TimeSpan.FromMinutes(8)).WithOperationDurationVariance(0.20),
                    new ResourceTool("Blade 8mm").WithOperationDurationAverage(TimeSpan.FromMinutes(10)).WithOperationDurationVariance(0.20)
                });

            var rsDrill = new ResourceGroup("Drill")
                .WithResourceuQuantity(1)
                .WithDefaultDurationMean(TimeSpan.FromMinutes(5))
                .WithDefaultDurationVariance(0.20)
                .WithTools(new List<ResourceTool> {
                    new ResourceTool("Head 10mm"),
                    new ResourceTool("Head 15mm"),
                });


            var rsAssembly = new ResourceGroup("Assembly")
                .WithResourceuQuantity(3)
                .WithTools(new List<ResourceTool> {
                    new ResourceTool("Screwing").WithOperationDurationAverage(TimeSpan.FromMinutes(5)).WithOperationDurationVariance(0.20),
                    new ResourceTool("Glueing").WithOperationDurationAverage(TimeSpan.FromMinutes(10)).WithOperationDurationVariance(0.20),
                    new ResourceTool("Plug").WithOperationDurationAverage(TimeSpan.FromMinutes(5)).WithOperationDurationVariance(0.20),
                    new ResourceTool("File").WithOperationDurationAverage(TimeSpan.FromMinutes(15)).WithOperationDurationVariance(0.20),
                });

            var rsColoring = new ResourceGroup("Paint")
                .WithResourceuQuantity(1)
                .WithTools(new List<ResourceTool> {
                    new ResourceTool("Grounding"),
                    new ResourceTool("Coloring"),
                });

            return new ResourceGroups().WithResourceGroup(new List<ResourceGroup> { rsSaw, rsDrill, rsAssembly, rsColoring })
                                                .WithDefaultOperationsDurationMean(TimeSpan.FromSeconds(300))
                                                .WithDefaultOperationsDurationVariance(0.20)
                                                .WithDefaultOperationsAmountMean(4)
                                                .WithDefaultOperationsAmountVariance(0.20); 
        }
    }
}
