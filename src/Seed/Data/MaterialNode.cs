using System;
using System.Collections.Generic;
using System.Text;

namespace Seed.Data
{
    public class MaterialNode
    {
        public MaterialNode()
        {
            IncomingEdges = new List<MaterialEdge>();
            OutgoingEdges = new List<MaterialEdge>();
            Operations = new List<MaterialNodeOperation>();
            Guid = Guid.NewGuid();
        }
        public Guid Guid { get; }
        public int InitialLevel { get; set; }
        public List<MaterialEdge> IncomingEdges { get; set; }
        public List<MaterialEdge> OutgoingEdges { get; set; }
        public List<MaterialNodeOperation> Operations { get; set; }
    }
}
