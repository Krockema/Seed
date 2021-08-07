using System;
using System.Collections.Generic;
using System.Text;

namespace Seed.Data
{
    public class Edge
    {
        private Node _from;
        private Node _to;
        public Node From { 
            get { return _from; }
            set {
                _from = value;
                _from.OutgoingEdge.Add(this);
                } 
            }
        public Node To { get { return _to; } 
            set {
                _to = value;
                _to.IncomingEdges.Add(this);
                } 
            }
    }
}
