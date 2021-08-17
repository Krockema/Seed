using Seed.Data;
using Seed.Distributions;
using Seed.Generator;
using Seed.Matrix;
using Seed.Parameter;
using Seed.Parameter.Operation;
using Seed.Parameter.TransitionMatrix;
using Seed.Test.DefaultConfiguration;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Seed.Test.OperationStructure
{
    public class OperationFixture : IDisposable
    {
        public Configuration Configuration { get; } = new();
        public RandomizerBase Randomizer { get; } = new RandomizerBase(1337);

        
        public ResourceGroupParameter ResourceGroups;
        public TransitionMatrix TransitionMatrix;
        public int MatrixSize = 4;
        public double OrganizationalDegree = 0.15;
        public double Lambda = 2;
        public OperationFixture()
        {
            Configuration = new Configuration();
            Configuration.WithOption(new TransitionMatrixParameter() { Lambda = 2, OrganizationalDegree = 0.15 });
            Configuration.WithOption(new MaterialStructureParameter() { ComplexityRatio = 3.1, ReuseRatio = 1.7, NumberOfSalesMaterials = 8, VerticalIntegration = 4 });
            Configuration.WithOption(Configuration.ReadFromFile<ResourceGroupParameter>("ExsampleResources.json"));
            var generator = new TransitionMatrixGenerator(Configuration);
            TransitionMatrix = generator.Generate();
            
            ResourceGroups = ConfigurationBase.CreateResourceGroups();
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
