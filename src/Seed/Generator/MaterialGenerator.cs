using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using Seed.Data;
using Seed.Distributions;
using Seed.Matrix;
using Seed.Parameter;
using Seed.Parameter.Material;
using Seed.Parameter.TransitionMatrix;

namespace Seed.Generator
{
    public class MaterialGenerator
    {
        public Materials Materials { get; init; } = new();
        private Queue<Edge> _unusedEdges = new Queue<Edge>();
        private readonly List<Edge> _edges = new ();
        private readonly IRandomizer _randomizer;
        private readonly int _verticalIntegration;
        private readonly int _complexityRatio;
        private readonly int _reuseRatio;
        private readonly int _salesMaterial;
        public MaterialGenerator(Configuration cfg, IRandomizer randomizer)
        {
            _verticalIntegration = cfg.Get<VerticalIntegration>().Value;
            _complexityRatio = cfg.Get<ComplexityRatio>().Value;
            _reuseRatio = cfg.Get<ReuseRatio>().Value;
            _salesMaterial = cfg.Get<SalesMaterial>().Value;
            _randomizer = randomizer;

            var resultingSalesMaterial = Math.Pow(_complexityRatio / _reuseRatio, _verticalIntegration - 1);
            if (resultingSalesMaterial > _salesMaterial)
            {
                throw new ArgumentException($"Invalid configuration; not enouth sales material output. Minimum is {resultingSalesMaterial}");
            }
            // step 1: Generate Materials
            // step 2: Generate Edges

        }
        
        public Materials CreateMaterials()
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
            var nodes = new NodeList();
            for (int i = 0; i < numberOfNodes; i++)
            {
                nodes.Add(new Node() { InitialLevel = currentLevel - 1 });
            }
            nodes.SaveNodes(); 
            Materials.Add(new(currentLevel, nodes));    
        }

        private List<Node> SalesMaterial()
        {
            return Materials[0].Nodes;
        }

        private List<Node> PurchaseMaterial()
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
                    _edges.Add(edge);
                }
            }

            for (int level = Materials.Count - 1; level >= 2; level--)
            {
                var currentLevelNodesWithoutEdges = Materials[level].Nodes;
                var decendingProbabilityMatrix = new ProbabilityByDistanceMatrix
                    (new MatrixSize(Materials.Count)
                    , new MatrixProbabilityByDistanceInitializerDescending());

                while (currentLevelNodesWithoutEdges.Count > 0)
                {
                    var edge = _unusedEdges.Dequeue();
                    var jumpTo = decendingProbabilityMatrix.GetRowJump(level, _randomizer.Next());
                    var higherLevelNodes = Materials[jumpTo].Nodes;
                    edge.From = currentLevelNodesWithoutEdges.DequeueNode();
                    edge.To = higherLevelNodes.GetNodeFromStorage(_randomizer.Next(higherLevelNodes.CountAll));
                    _edges.Add(edge);
                }

            }
            DistributeRemainingEdges();
        }


        private void CreateDivergentStructure()
        {
            var stages = Materials.Count;
            for (int level = stages - 1; level >= 2; level--)
            {
                var currentLevelNodesWithoutEdges = Materials[level].Nodes;
                var higherLevelNodes = Materials[level - 1].Nodes;

                while (currentLevelNodesWithoutEdges.Count > 0)
                {
                    var edge = _unusedEdges.Dequeue();
                    edge.From = currentLevelNodesWithoutEdges.DequeueNode();
                    edge.To = higherLevelNodes.GetNodeAt(_randomizer.Next(currentLevelNodesWithoutEdges.Count()));
                    _edges.Add(edge);
                }
            }

            for (int level = 0; level < Materials.Count; level++)
            {
                var currentLevelNodesWithoutEdges = Materials[level].Nodes;
                var probabilityMatrix =
                    new ProbabilityByDistanceMatrix
                        (new MatrixSize(stages), new MatrixProbabilityByDistanceInitializerAscending(stages));

                while (currentLevelNodesWithoutEdges.Count > 0)
                {
                    var edge = _unusedEdges.Dequeue();
                    var jumpTo = probabilityMatrix.GetRowJump(level, _randomizer.Next());
                    var lowerLevelNodes = Materials[jumpTo].Nodes;
                    edge.From = lowerLevelNodes.GetNodeFromStorage(_randomizer.Next(lowerLevelNodes.CountAll));
                    edge.To = currentLevelNodesWithoutEdges.DequeueNode();
                    _edges.Add(edge);
                }
            }
            DistributeRemainingEdges();
        }


        private void DistributeRemainingEdges()
        {
            if (_unusedEdges.Count == 0)
                return;

            var decendingProbabilityMatrix = new ProbabilityByDistanceMatrix
                (new MatrixSize(Materials.Count)
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
                _edges.Add(edge);
            }
        }


        public Queue<Edge> CreateEdges()
        {
            // reuse * (all nodes substracted by the number of nodes from first level (sales))
            double maxReuseUseEdge = _reuseRatio * (Materials.Sum(x => x.Nodes.Count()) - SalesMaterial().Count);
            // reuse * (all nodes substracted by the number of nodes last first level (purchase))
            double maxComplexityRatio = _complexityRatio * (Materials.Sum(x => x.Nodes.Count()) - PurchaseMaterial().Count());
            // Take the bigger number
            var edgeCount = Convert.ToInt32(Math.Round(Math.Max(maxReuseUseEdge, maxComplexityRatio), 0));
            // Create a set of empty edges acoding to edgeCount
            _unusedEdges = new Queue<Edge>(
                                from edge in Enumerable.Range(0, edgeCount) 
                                select new Edge());
            return _unusedEdges;
        }
    }
}
