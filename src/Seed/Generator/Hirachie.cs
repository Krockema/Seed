using Seed.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Seed.Generator
{
    public class Hirachie 
    {
        private Tuple<int, NodeList> _level;
        public Hirachie(int level, NodeList nodes)
        {
            _level = new Tuple<int, NodeList>(level, nodes);
        }

        public int Level => _level.Item1;
        public NodeList Nodes => _level.Item2;
    }
}
