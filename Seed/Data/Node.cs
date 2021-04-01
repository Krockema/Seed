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
            OutgoingEdge = new List<Edge>();
        }
        public int InitialLevel { get; set; }
        public List<Edge> IncomingEdges { get; set; }
        public List<Edge> OutgoingEdge { get; set; }
    }
}
