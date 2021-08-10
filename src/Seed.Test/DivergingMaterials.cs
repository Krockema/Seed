using Seed.Generator;
using Seed.Parameter.Material;
using System;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace Seed.Test
{
    public class DivergingMaterials : IClassFixture<MaterialFixture>
    {
        private MaterialFixture _materialFixture;
        private VerticalIntegration _verticalIntegration = new (4);
        private ComplexityRatio _complexityRatio = new (2);
        private ReuseRatio _reuseRatio = new (4);
        private SalesMaterial _salesMaterial = new (8);
        private ITestOutputHelper _out;

        public DivergingMaterials(MaterialFixture materialFixture, ITestOutputHelper outputHelper)
        {
            _out = outputHelper;
            _materialFixture = materialFixture;
            _materialFixture.Configuration.ReplaceOption(_verticalIntegration);
            _materialFixture.Configuration.ReplaceOption(_complexityRatio);
            _materialFixture.Configuration.ReplaceOption(_reuseRatio);
            _materialFixture.Configuration.ReplaceOption(_salesMaterial);
            _materialFixture.Generate();
        }

        [Fact]
        public void NumberOfMaterialsPerLevel()
        {
            double purchaseMaterials = _materialFixture.Materials.NodesPurchaseOnly().Length;
            double salesMaterials = _materialFixture.Materials.NodesSalesOnly().Length;
            foreach (var materialHirarchie in _materialFixture.Materials)
            {
                var expected = 0.0;
                if (materialHirarchie.Level == 1)
                    expected = purchaseMaterials / (Math.Pow((double)_complexityRatio.Value / _reuseRatio.Value, _verticalIntegration.Value - 1));
                else 
                    expected = salesMaterials * (Math.Pow((double)_complexityRatio.Value / _reuseRatio.Value, materialHirarchie.Level - 1));

                Assert.Equal(Math.Round(expected, 0), Materials.NodesInUse.Count(y => y.InitialLevel + 1 == materialHirarchie.Level));
            }
        }

        [Fact]
        public void NumberOfLevels()
        {
            Assert.Equal(4, _materialFixture.Materials.Count());
        }

        [Fact]
        public void NumberOfEdgesPerLevel()
        {
            Assert.Equal(28, _materialFixture.Edges.Count());
        }
        [Fact]
        public void NoEdgesNoNodesLeftBehind()
        {
            Assert.Empty(_materialFixture.InitialEdges);
        }
        [Fact(Skip = "Manual")]
        public void ShowNodesStructures()
        {

            var sales = _materialFixture.Materials.NodesSalesOnly();
            var purchase = _materialFixture.Materials.NodesPurchaseOnly();
            _out.WriteLine($"> {sales.Count()} Sales Materials");
            _out.WriteLine($"> {purchase.Count()} Purchase Materials ");

            foreach (var node in sales)
            {
                _out.WriteLine($"> {node.InitialLevel} {node.Guid}");
                _materialFixture.solveStructure(node.IncomingEdges.ToArray(), 2);
            }
        }
        
        [Fact]
        public void DegreeOfCoplexityAndMultipleUse()
        {
            var allMats = _materialFixture.Materials.ToNodeArray;
            var totalMats = allMats.Count();
            var matsWithSuccessor = allMats.Sum(x => x.OutgoingEdges.Count);
            var matsSalesOnly = _materialFixture.Materials.NodesSalesOnly().Count();
            var multipleUse = (double)matsWithSuccessor / (totalMats- matsSalesOnly);
            _out.WriteLine($" Multiple Use : {multipleUse}");
            Assert.Equal(_reuseRatio.Value, multipleUse);

            var matsWithPredecessor = allMats.Sum(x => x.IncomingEdges.Count);
            var matsPurchaseOnly = _materialFixture.Materials.NodesPurchaseOnly().Count();
            var degreeOfComplexity = (double)matsWithPredecessor / (totalMats- matsPurchaseOnly);
            _out.WriteLine($" Complexity : {degreeOfComplexity}");
            Assert.Equal(_complexityRatio.Value, degreeOfComplexity);
        } 
    }
}
