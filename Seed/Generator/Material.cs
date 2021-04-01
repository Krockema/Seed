using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MathNet.Numerics.Random;
using Seed.Data;
using Seed.Parameter;
using Seed.Parameter.Material;

namespace Seed.Generator
{
    public class Material
    {
        private readonly List<Tuple<int, List<Node>>> _materials = new List<Tuple<int, List<Node>>>();
        private readonly List<Edge> _edges = new List<Edge>();
        private readonly RandomSource _randomSource;
        private readonly int _verticalIntegration;
        private readonly int _complexityRatio;
        private readonly int _reuseRatio;
        private readonly int _salesMaterial;
        public Material(Configuration cfg, RandomSource randomSource)
        {
            _verticalIntegration = cfg.Get<VerticalIntegration>().Value;
            _complexityRatio = cfg.Get<ComplexityRatio>().Value;
            _reuseRatio = cfg.Get<ReuseRatio>().Value;
            _salesMaterial = cfg.Get<SalesMaterial>().Value;
            _randomSource = randomSource;
        }

        public void CreateMaterials()
        {
            foreach (var level in Enumerable.Range(1, _verticalIntegration))
            {
                // calc number of Parts for given lvl
                var numberOfMaterials = Math.Round(Math.Pow((double)_complexityRatio / _reuseRatio, level - 1) * _salesMaterial, 0);

                if (numberOfMaterials > 0)
                    CreateNodes(Convert.ToInt32(numberOfMaterials), level);
            }
        }

        private void CreateNodes(int numberOfNodes, int currentLevel)
        {
            var nodes = new List<Node>();
            for (int i = 0; i < numberOfNodes; i++)
            {
                nodes.Add(new Node() { InitialLevel = currentLevel});
            }
            _materials.Add(new Tuple<int, List<Node>>(currentLevel, nodes));    
        }

        private List<Node> SalesMaterial()
        {
            return _materials[0].Item2;
        }

        private List<Node> PurchaseMaterial()
        {
            return _materials[_materials.Count() - 1].Item2;
        }

        public void CreateEdges()
        {
            if (_reuseRatio < _complexityRatio)
            {
                CreateConvergentStructure();
                return;
            }

            CreateDivergentStructure();
        }

        private void CreateDivergentStructure()
        {
            foreach (var level in Enumerable.Range(0, _materials.Count - 2))
            {
                var currentLevelNodesWithoutEdges = new Queue<Node>(_materials[level].Item2);
                var nextLevelNodes = _materials[level + 1].Item2;
                while (currentLevelNodesWithoutEdges.Count > 0)
                {
                    var to = currentLevelNodesWithoutEdges.Dequeue();
                    var from = nextLevelNodes[_randomSource.Next(currentLevelNodesWithoutEdges.Count - 1)];
                    var edge = new Edge() {From = from, To = to};
                    _edges.Add(edge);
                    to.IncomingEdges.Add(edge);
                    from.OutgoingEdge.Add(edge);
                }
                
                



            }
        }

        private void CreateConvergentStructure()
        {
            throw new NotImplementedException();
        }
    }
}
