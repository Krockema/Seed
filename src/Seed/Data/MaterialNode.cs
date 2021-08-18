using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Seed.Data
{
    public class MaterialNode
    {
        [JsonIgnore]
        private static int IdCounter = 0;
        public MaterialNode()
        {
            IncomingEdges = new List<MaterialEdge>();
            OutgoingEdges = new List<MaterialEdge>();
            Operations = new List<MaterialNodeOperation>();
            Id = IdCounter++;
            Cost = 1;
        }
        public int Id { get; }
        public int InitialLevel { get; set; }
        [JsonIgnore]
        public List<MaterialEdge> IncomingEdges { get; set; }
        [JsonIgnore]
        public List<MaterialEdge> OutgoingEdges { get; set; }
        [JsonIgnore]
        public List<MaterialNodeOperation> Operations { get; set; }
        public double Cost { get; set;}
        public List<int> IncomingEdgeIds => IncomingEdges.Select(x => x.Id).ToList();
        public List<int> OutgoingEdgeIds => IncomingEdges.Select(x => x.Id).ToList();
    }
}
