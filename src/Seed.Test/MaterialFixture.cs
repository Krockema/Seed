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

namespace Seed.Test
{
    public class MaterialFixture : IDisposable
    {
        public Configuration Configuration { get; } = new();
        public Randomizer Randomizer { get; } = new Randomizer(29);
        public Materials Materials { get; private set; } 
        public Edge[] Edges { get; private set;}
        public Queue<Edge> InitialEdges { get; private set; }
        public Action<Edge[], int> solveStructure { get; } = null;
        public MaterialFixture()
        {
            solveStructure = (edges, lvl) =>
            {
                foreach (var edge in edges)
                {
                    var intend = "".PadLeft(lvl, '-');
                    System.Diagnostics.Debug.WriteLine(intend + $"> {edge.From.InitialLevel} {edge.From.Guid}");
                    solveStructure(edge.From.IncomingEdges.ToArray(), lvl + 2);
                }
            };
        }
        public void Generate()
        {
            var matGenerator = new MaterialGenerator(Configuration, Randomizer);
            Materials = matGenerator.CreateMaterials();
            InitialEdges = matGenerator.CreateEdges();
            Edges = InitialEdges.ToArray();
            matGenerator.ConnectEdges();
        }

        public void Dispose() { }

  
    }
}
