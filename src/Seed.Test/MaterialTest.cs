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
using System.Xml.Linq;
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
        public void ThrowOnInvalidConfigToFewSalesMaterial(int complexity, int reuse, int salesMaterials)
        {

            configuration = new();
            configuration.WithOption(new VerticalIntegration(4));
            configuration.WithOption(new ComplexityRatio(complexity));
            configuration.WithOption(new ReuseRatio(reuse));
            configuration.WithOption(new SalesMaterial(salesMaterials));
            Assert.Throws<ArgumentException>(() => new MaterialGenerator(configuration, random));
        }

        [Theory]
        [InlineData(4, 2, 8)]
        public void ShowNodesStructures(int complexity, int reuse, int salesMaterials)
        {

            configuration = new();
            configuration.WithOption(new VerticalIntegration(4));
            configuration.WithOption(new ComplexityRatio(complexity));
            configuration.WithOption(new ReuseRatio(reuse));
            configuration.WithOption(new SalesMaterial(salesMaterials));
            var matGenerator = new MaterialGenerator(configuration, random);
            materials = matGenerator.CreateMaterials();
            edges = matGenerator.CreateEdges();
            matGenerator.ConnectEdges();
            Assert.Empty(edges);


            var sales = matGenerator.Materials.NodesSalesOnly();
            var purchase = matGenerator.Materials.NodesPurchaseOnly();
            System.Diagnostics.Debug.WriteLine($"> {sales.Count()} Sales Materials");
            System.Diagnostics.Debug.WriteLine($"> {purchase.Count()} Purchase Materials ");

            foreach (var node in sales)
            {
                System.Diagnostics.Debug.WriteLine($"> {node.InitialLevel} {node.Guid}");
                SolveNodeStructure(node.IncomingEdges.ToArray(), 2);
            }


        }

        public void SolveNodeStructure(Edge[] edges, int lvl)
        {
            foreach (var edge in edges)
            {
                var intend = "".PadLeft(lvl, '-');
                System.Diagnostics.Debug.WriteLine(intend + $"> {edge.From.InitialLevel} {edge.From.Guid}");
                SolveNodeStructure(edge.From.IncomingEdges.ToArray(), lvl + 2);
            }
        }


    }
}
