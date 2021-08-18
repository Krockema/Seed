using Seed.Parameter;
using Seed.Parameter.Operation;
using Seed.Parameter.TransitionMatrix;

namespace Seed.Test.DefaultConfiguration
{
    public class ConfigurationBase
    {
        public static Configuration Default() {
            var config = new Configuration(); 
            var mat = new MaterialConfig()
            {
                StructureParameter = new StructureParameter() { ComplexityRatio = 4, ReuseRatio = 2, NumberOfSalesMaterials = 8, VerticalIntegration = 4 },
                TransitionMatrixParameter = new TransitionMatrixParameter() { Lambda = 2, OrganizationalDegree = 0.15 }
            };
            config.WithOption(mat);            
            return config;
        }
        public static ResourceConfig CreateResourceConfig()
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

            return new ResourceConfig().WithResourceGroup(new List<ResourceGroup> { rsSaw, rsDrill, rsAssembly, rsColoring })
                                                .WithDefaultOperationsDurationMean(TimeSpan.FromSeconds(300))
                                                .WithDefaultOperationsDurationVariance(0.20)
                                                .WithDefaultOperationsAmountMean(4)
                                                .WithDefaultOperationsAmountVariance(0.20); 
        }
    }
}
