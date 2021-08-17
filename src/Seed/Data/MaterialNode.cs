using System.Collections.Generic;

namespace Seed.Data
{
    public class MaterialNode
    {
        private static int IdCounter = 0;
        public MaterialNode()
        {
            IncomingEdges = new List<MaterialEdge>();
            OutgoingEdges = new List<MaterialEdge>();
            Operations = new List<MaterialNodeOperation>();
            Id = IdCounter++;
        }
        public int Id { get; }
        public int InitialLevel { get; set; }
        public List<MaterialEdge> IncomingEdges { get; set; }
        public List<MaterialEdge> OutgoingEdges { get; set; }
        public List<MaterialNodeOperation> Operations { get; set; }
    }
}
