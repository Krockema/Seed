using Seed.Data;
using Seed.Distributions;
using Seed.Generator.Operation;
using Seed.Matrix;
using Seed.Parameter;
using Seed.Test.DefaultConfiguration;
using System;
using System.Linq;

namespace Seed.Test.OperationStructure
{
    public class OperationFixture : IDisposable
    {
        public Configuration Configuration { get; } = new();
        public RandomizerBase Randomizer { get; } = new RandomizerBase(1337);

        
        public ResourceConfig ResourceConfig;
        public TransitionMatrix TransitionMatrix;
        public int MatrixSize = 4;
        public double OrganizationalDegree = 0.15;
        public double Lambda = 2;
        public OperationFixture()
        {
            Configuration = new Configuration();
            Configuration.WithOption(Configuration.ReadFromFile<MaterialConfig>("MaterialConfig.json"));
            Configuration.WithOption(Configuration.ReadFromFile<ResourceConfig>("ResourceConfig.json"));
            TransitionMatrix = TransitionMatrixGenerator.WithConfiguration(Configuration).Generate();
            ResourceConfig = ConfigurationBase.CreateResourceConfig();
        }

        public MaterialNode[] CreateMaterials(int quantity)
        {
            MaterialNode[] materials = new MaterialNode[quantity];
            foreach (var i in Enumerable.Range(0, quantity))
            {
                materials[i] = new MaterialNode();
            }
            return materials;
        }

        public void Dispose() { }
    }
}
