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

        
        public ResourceGroups ResourceGroups;
        public TransitionMatrix TransitionMatrix;
        public MatrixSize MatrixSize = new (4);
        public OrganizationalDegree OrganizationalDegree = new (0.15);
        public Lambda Lambda = new (2);
        public OperationFixture()
        {
            this.Configuration.WithOption(MatrixSize);
            this.Configuration.WithOption(OrganizationalDegree);
            this.Configuration.WithOption(Lambda);

            var generator = new TransitionMatrixGenerator(Configuration, Randomizer);
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
