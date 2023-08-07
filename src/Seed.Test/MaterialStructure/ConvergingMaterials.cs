using Seed.Generator;
using Seed.Parameter;
using Seed.Parameter.TransitionMatrix;
using System;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace Seed.Test.MaterialStructure
{
    public class ConvergingMaterials : IClassFixture<MaterialFixture>
    {
        private MaterialFixture _materialFixture;
        private ITestOutputHelper _out;
        private TransitionMatrixParameter tmp = new TransitionMatrixParameter() { Lambda = 2, OrganizationalDegree = 0.80 };
        private StructureParameter msp = new StructureParameter() { ComplexityRatio = 4
                                                                    , ReuseRatio = 2
                                                                    , NumberOfSalesMaterials = 50
                                                                    , VerticalIntegration = 2 };
        public ConvergingMaterials(MaterialFixture materialFixture, ITestOutputHelper outputHelper)
        {
            _out = outputHelper;
            _materialFixture = materialFixture;
            _materialFixture.MaterialConfiguration = new MaterialConfig{ StructureParameter = msp, TransitionMatrixParameter = tmp };
            _materialFixture.GenerateMaterials();
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
                    expected = purchaseMaterials / (Math.Pow((double)msp.ComplexityRatio / msp.ReuseRatio, msp.VerticalIntegration - 1));
                else 
                    expected = salesMaterials * (Math.Pow((double)msp.ComplexityRatio / msp.ReuseRatio, materialHirarchie.Level - 1));

                Assert.Equal(Math.Round(expected, 0), _materialFixture.Materials.NodesInUse.Count(y => y.InitialLevel + 1 == materialHirarchie.Level));
            }
        }

        [Fact]
        public void NumberOfLevels()
        {
            Assert.Equal(4, _materialFixture.Materials.Count());
        }

        [Fact]
        public void NumberOfEdges()
        {
            Assert.Equal(1400, _materialFixture.Edges.Count());
        }
        [Fact]
        public void NoEdgesNoNodesLeftBehind()
        {
            Assert.Equal(_materialFixture.Materials.Edges.Count, _materialFixture.InitialEdges);
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
                _out.WriteLine($"> {node.InitialLevel} {node.Id}");
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
            var multipleUse = (double)matsWithSuccessor / (totalMats - matsSalesOnly);
            _out.WriteLine($" Multiple Use : {multipleUse}");
            Assert.Equal(msp.ReuseRatio, multipleUse);

            var matsWithPredecessor = allMats.Sum(x => x.IncomingEdges.Count);
            var matsPurchaseOnly = _materialFixture.Materials.NodesPurchaseOnly().Count();
            var degreeOfComplexity = (double)matsWithPredecessor / (totalMats- matsPurchaseOnly);
            _out.WriteLine($" Complexity : {degreeOfComplexity}");
            Assert.Equal(msp.ComplexityRatio, degreeOfComplexity);
        } 
    }
}
