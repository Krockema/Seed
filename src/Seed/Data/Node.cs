using System;
using System.Collections.Generic;
using System.Text;

namespace Seed.Data
{
    public class Node
    {
        public Node()
        {
            IncomingEdges = new List<Edge>();
            OutgoingEdges = new List<Edge>();
            Guid = Guid.NewGuid();
        }
        public Guid Guid { get; }
        public int InitialLevel { get; set; }
        public List<Edge> IncomingEdges { get; set; }
        public List<Edge> OutgoingEdges { get; set; }
    }
}
