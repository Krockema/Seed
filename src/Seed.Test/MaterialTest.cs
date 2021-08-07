using Seed.Data;
using Seed.Distributions;
using Seed.Generator;
using Seed.Parameter;
using Seed.Parameter.Material;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Seed.Test
{
    public class MaterialTest
    {
        Configuration configuration = new();
        Randomizer random = new Randomizer(29);
        ListExtension<Hirachie> materials;
        Queue<Edge> edges;

        public MaterialTest()
        {
            configuration.WithOption(new VerticalIntegration(4));
            configuration.WithOption(new ComplexityRatio(1));
            configuration.WithOption(new ReuseRatio(2));
            configuration.WithOption(new SalesMaterial(4));
            var matGenerator = new MaterialGenerator(configuration, random);
            // materials = matGenerator.CreateMaterials();
            // edges = matGenerator.CreateEdges();
            // matGenerator.ConnectEdges();
        }

        [Fact]
        public void NumberOfMaterialsPerLevelConverging()
        {
            materials.ForEach(x => Assert.Equal(Math.Pow(2, x.Level+1), x.Nodes.Count()));
            Assert.Equal(4, materials.Count());
        }

        [Fact]
        public void NumberOfEdgesPerLevelConverging()
        {
            Assert.Equal(56, edges.Count());
        }

        [Theory]
        [InlineData(2,4)]
        [InlineData(4, 2)]
        public void NoEdgesNoNodesLeftBehind(int complexity, int reuse)
        {
            configuration = new();
            configuration.WithOption(new VerticalIntegration(4));
            configuration.WithOption(new ComplexityRatio(complexity));
            configuration.WithOption(new ReuseRatio(reuse));
            configuration.WithOption(new SalesMaterial(8));
            var matGenerator = new MaterialGenerator(configuration, random);
            materials = matGenerator.CreateMaterials();
            edges = matGenerator.CreateEdges();
            matGenerator.ConnectEdges();
            Assert.Empty(edges);
        }
        
        [Theory]
        [InlineData(4, 2, 4)]
        public void ThrowOnInvalidConfigToFewSalesMaterial(int complexity, int reuse, int _verticalIntegration)
        {

            configuration = new();
            configuration.WithOption(new VerticalIntegration(4));
            configuration.WithOption(new ComplexityRatio(complexity));
            configuration.WithOption(new ReuseRatio(reuse));
            configuration.WithOption(new SalesMaterial(_verticalIntegration));
            Assert.Throws<ArgumentException>(() => new MaterialGenerator(configuration, random));
        }

    }
}
