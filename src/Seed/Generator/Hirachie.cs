using Seed.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Seed.Generator
{
    public class Hirachie 
    {
        private Tuple<int, ListExtension<Node>> _level;
        public Hirachie(int level, ListExtension<Node> nodes)
        {
            _level = new Tuple<int, ListExtension<Node>>(level, nodes);
        }

        public int Level => _level.Item1;
        public ListExtension<Node> Nodes => _level.Item2;
    }
}
