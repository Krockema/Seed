using System;
using System.Collections.Generic;
using System.Text;

namespace Seed.Data
{
    public class MaterialEdge
    {
        private MaterialNode _from;
        private MaterialNode _to;
        public MaterialNode From { 
            get { return _from; }
            set {
                _from = value;
                _from.OutgoingEdges.Add(this);
                } 
            }
        public MaterialNode To { get { return _to; } 
            set {
                _to = value;
                _to.IncomingEdges.Add(this);
                } 
            }
    }
}
