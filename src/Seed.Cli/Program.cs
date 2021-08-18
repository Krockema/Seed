using Microsoft.Extensions.Configuration;
using Seed.Cli;
using Seed.Distributions;
using Seed.Generator.Material;
using Seed.Generator.Operation;
using Seed.Parameter;
using System.Text.Json;

namespace Seed
{
    class Program
    {
        public static bool Quiet;
        static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder();
            builder.AddCommandLine(args, new Mappings());
            var cliConfig = builder.Build();

            Quiet = cliConfig["Quiet"] != null;

            // New config from files
            var seedConfig = new Configuration();
            
            WriteIf(Quiet, $"Read material config from: '{cliConfig["MaterialConfig"]}'");
            seedConfig.WithOption(Configuration.ReadFromFile<MaterialConfig>(cliConfig["MaterialConfig"]));

            WriteIf(Quiet, $"Read resource config from: '{cliConfig["ResourceConfig"]}'");
            seedConfig.WithOption(Configuration.ReadFromFile<ResourceConfig>(cliConfig["ResourceConfig"]));

            // Generating Materials
            StartGenerating(seedConfig, cliConfig);

            WriteIf(Quiet, $"Wrote results to '{cliConfig["OutputPath"]}'");

            if (!Quiet)
            {
                WriteIf(Quiet, $"Press any key to continiue. . .");
                Console.ReadKey();
            }
            
        }

        // read config
            public static void StartGenerating(Configuration seedConfig, IConfigurationRoot cliConfig)
        {
            var materialConfig = seedConfig.Get<MaterialConfig>();
            var resourceConfig = seedConfig.Get<ResourceConfig>();
            var materials = MaterialGenerator.WithConfiguration(materialConfig)
                                             .Generate();

            var randomizer = new RandomizerBase(materialConfig.StructureParameter.Seed);
            var randomizerCollection = RandomizerCollection.WithTransition(randomizer)
                                                .WithOperationDuration(new RandomizerLogNormal(randomizer.Next(int.MaxValue)))
                                                .WithOperationAmount(new RandomizerBinominial(randomizer.Next(int.MaxValue)))
                                                .Build();


            var transitionMatrix = TransitionMatrixGenerator.WithConfiguration(seedConfig).Generate();


            var operationDistributor = OperationDistributor.WithTransitionMatrix(transitionMatrix)
                                                            .WithRandomizerCollection(randomizerCollection)
                                                            .WithResourceConfig(resourceConfig)
                                                            .WithMaterials(materials)
                                                            .Build();
            
            var operationGenerator = OperationGenerator.WithOperationDistributor(operationDistributor)
                                                       .WithMaterials(materials.NodesWithoutPurchase())
                                                       .Generate();

            // some statistics
            var group = materials.Operations.GroupBy(x => new { x.Node.Id }).Select(g => new { Key = g.Key, Count = g.Count() });
            var average = group.Average(x => x.Count);
            WriteIf(Quiet, materials.Operations.Count + " operations created");
            WriteIf(Quiet, average + " average operations distributed over " + group.Count() + " materials.");
            WriteIf(Quiet, transitionMatrix.GetOrganizationalDegree() + " organizational degree on generated matrix");
            WriteIf(Quiet, operationGenerator.GetGeneratedTransitionMatrix().Normalize().GetOrganizationalDegree() + " organizational degree on generated materials");

            Output(materials.NodesInUse, "Nodes.json", cliConfig["OutputPath"]);
            Output(materials.Edges, "Edges.json", cliConfig["OutputPath"]);
            Output(materials.Operations, "Operations.json", cliConfig["OutputPath"]);
            

        }
        public static void WriteIf(bool silent, string line)
        {
            if (silent) return;
            Console.WriteLine(line);
        }

        public static void Output(object toSerialize, string fileName, string outDir)
        {
            if (!Directory.Exists(outDir))
                Directory.CreateDirectory(outDir);
            
            var outJson = JsonSerializer.Serialize(toSerialize, new JsonSerializerOptions() { WriteIndented = true } );
            File.WriteAllText(Path.Combine(Environment.CurrentDirectory , outDir, fileName), outJson);
        }
    }
}
