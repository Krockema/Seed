using Seed.Data;
using Seed.Distributions;
using Seed.Matrix;
using Seed.Parameter;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Seed.Generator.Material
{
    public class MaterialGenerator : IWithMaterialConfiguration
    {
        public Materials Materials { get; init; } = new();
        private MaterialConfig materialParameter;
        private Queue<MaterialEdge> _unusedEdges { get; set; }  = new Queue<MaterialEdge>();
        public int InitialEdgeCount { get; private set; }
        private readonly IRandomizer _randomizer;
        private int _verticalIntegration => materialParameter.StructureParameter.VerticalIntegration;
        private double _complexityRatio => materialParameter.StructureParameter.ComplexityRatio;
        private double _reuseRatio => materialParameter.StructureParameter.ReuseRatio;
        private int _salesMaterial => materialParameter.StructureParameter.NumberOfSalesMaterials;
        private MaterialGenerator(MaterialConfig materialConfig, IRandomizer randomizer)
        {
            materialParameter = materialConfig;
            
            _randomizer = randomizer;

            var resultingSalesMaterial = Math.Pow(_complexityRatio / _reuseRatio, _verticalIntegration - 1);
            if (resultingSalesMaterial > _salesMaterial)
            {
                throw new ArgumentException($"Invalid configuration; not enouth sales material output. Minimum is {resultingSalesMaterial}");
            }
            // step 1: Generate Materials
            // step 2: Generate Edges

        }

        public static IWithMaterialConfiguration WithConfiguration(MaterialConfig materialConfig)
        {
            var rnd = new RandomizerBase(materialConfig.StructureParameter.Seed);
            var matGenerator = new MaterialGenerator(materialConfig, rnd);
            return matGenerator;
        }

        public Materials Generate()
        {
            var materials = this.CreateMaterials();
            this.CreateEdges();
            this.ConnectEdges();
            Materials.CalculateCosts(materials.NodesSalesOnly().SelectMany(x => x.IncomingEdges).ToArray());
            return materials;
        }


        private Materials CreateMaterials()
        {
            foreach (var level in Enumerable.Range(1, _verticalIntegration))
            {
                // calc number of Parts for given lvl
                var numberOfMaterials = Math.Round(Math.Pow((double)_complexityRatio / _reuseRatio, level - 1) * _salesMaterial, 0);

                if (numberOfMaterials > 0)
                    CreateNodes(Convert.ToInt32(numberOfMaterials), level);
            }
            return Materials;
        }

        private void CreateNodes(int numberOfNodes, int currentLevel)
        {
            var nodes = new MaterialNodeList(Materials);
            for (int i = 0; i < numberOfNodes; i++)
            {
                nodes.Add(new MaterialNode() { InitialLevel = currentLevel - 1 });
            }
            nodes.SaveNodes(); 
            Materials.Add(new(currentLevel, nodes));    
        }

        private List<MaterialNode> SalesMaterial()
        {
            return Materials[0].Nodes;
        }

        private List<MaterialNode> PurchaseMaterial()
        {
            return Materials[Materials.Count() - 1].Nodes;
        }

        public void ConnectEdges()
        {
            if (_reuseRatio < _complexityRatio)
            {
                CreateConvergentStructure();
                return;
            }
            CreateDivergentStructure();
        }

        private void CreateConvergentStructure()
        {
            for (int level = 0; level < Materials.Count -1; level++)
            {
                var currentLevelNodesWithoutEdges = Materials[level].Nodes;
                var lowerLevelNodes = Materials[level + 1].Nodes;

                while (currentLevelNodesWithoutEdges.Count > 0)
                {
                    var edge = _unusedEdges.Dequeue();
                    edge.From = lowerLevelNodes.GetNodeAt(_randomizer.Next(currentLevelNodesWithoutEdges.Count()));
                    edge.To = currentLevelNodesWithoutEdges.DequeueNode();
                    Materials.Edges.Add(edge);
                }
            }

            for (int level = Materials.Count - 1; level >= 1; level--)
            {
                var currentLevelNodesWithoutEdges = Materials[level].Nodes;
                var decendingProbabilityMatrix = new ProbabilityByDistanceMatrix(
                     Materials.Count
                    , new MatrixProbabilityByDistanceInitializerDescending());

                while (currentLevelNodesWithoutEdges.Count > 0)
                {
                    var edge = _unusedEdges.Dequeue();
                    var jumpTo = decendingProbabilityMatrix.GetRowJump(level, _randomizer.Next());
                    var higherLevelNodes = Materials[jumpTo].Nodes;
                    edge.From = currentLevelNodesWithoutEdges.DequeueNode();
                    edge.To = higherLevelNodes.GetNodeFromStorage(_randomizer.Next(higherLevelNodes.CountAll));
                    Materials.Edges.Add(edge);
                }

            }
            DistributeRemainingEdges();
        }


        private void CreateDivergentStructure()
        {
            var stages = Materials.Count;
            for (int level = stages - 1; level >= 1; level--)
            {
                var currentLevelNodesWithoutEdges = Materials[level].Nodes;
                var higherLevelNodes = Materials[level - 1].Nodes;

                while (currentLevelNodesWithoutEdges.Count > 0)
                {
                    var edge = _unusedEdges.Dequeue();
                    edge.From = currentLevelNodesWithoutEdges.DequeueNode();
                    edge.To = higherLevelNodes.GetNodeAt(_randomizer.Next(higherLevelNodes.Count()));
                    Materials.Edges.Add(edge);
                }
            }

            for (int level = 0; level < Materials.Count; level++)
            {
                var currentLevelNodesWithoutEdges = Materials[level].Nodes;
                var probabilityMatrix =
                    new ProbabilityByDistanceMatrix
                        (stages, new MatrixProbabilityByDistanceInitializerAscending(stages));

                while (currentLevelNodesWithoutEdges.Count > 0)
                {
                    var edge = _unusedEdges.Dequeue();
                    var jumpTo = probabilityMatrix.GetRowJump(level, _randomizer.Next());
                    var lowerLevelNodes = Materials[jumpTo].Nodes;
                    edge.From = lowerLevelNodes.GetNodeFromStorage(_randomizer.Next(lowerLevelNodes.CountAll));
                    edge.To = currentLevelNodesWithoutEdges.DequeueNode();
                    Materials.Edges.Add(edge);
                }
            }
            DistributeRemainingEdges();
        }


        private void DistributeRemainingEdges()
        {
            if (_unusedEdges.Count == 0)
                return;

            var decendingProbabilityMatrix = new ProbabilityByDistanceMatrix
                (Materials.Count
                , new MatrixProbabilityByDistanceInitializerDescending());
            
            // caus we are looking from top to bottom, lower than purchase is not possible
            var nodesToDrawFrom = Materials.NodesWithoutSales();

            while (_unusedEdges.Count>0)
            {
                var edge = _unusedEdges.Dequeue();
                // get Random node from all non purchase nodes
                edge.From = nodesToDrawFrom[_randomizer.Next(nodesToDrawFrom.Length)];
                // get target Row
                var targetLevel = decendingProbabilityMatrix.GetRowJump(edge.From.InitialLevel, _randomizer.Next());
                var numberOfNodes = Materials.CountDequeuedNodesFor(targetLevel);
                edge.To = Materials.GetNodeInUseBy(targetLevel, _randomizer.Next(numberOfNodes));
                Materials.Edges.Add(edge);
            }
        }


        public Queue<MaterialEdge> CreateEdges()
        {
            // reuse * (all nodes substracted by the number of nodes from first level (sales))
            double maxReuseUseEdge = _reuseRatio * (Materials.Sum(x => x.Nodes.Count()) - SalesMaterial().Count);
            // reuse * (all nodes substracted by the number of nodes last first level (purchase))
            double maxComplexityRatio = _complexityRatio * (Materials.Sum(x => x.Nodes.Count()) - PurchaseMaterial().Count());
            // Take the bigger number
            var edgeCount = Convert.ToInt32(Math.Round(Math.Max(maxReuseUseEdge, maxComplexityRatio), 0));
            // Create a set of empty edges acoding to edgeCount
            _unusedEdges = new Queue<MaterialEdge>(
                                from edge in Enumerable.Range(0, edgeCount) 
                                select new MaterialEdge());
            InitialEdgeCount = _unusedEdges.Count();
            return _unusedEdges;
        }
    }
}
