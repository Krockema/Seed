using Seed.Data;
using System;

namespace Seed.Generator.Material
{
    public class MaterialHirachie 
    {
        private Tuple<int, MaterialNodeList> _level;
        public MaterialHirachie(int level, MaterialNodeList nodes)
        {
            _level = new Tuple<int, MaterialNodeList>(level, nodes);
        }

        public int Level => _level.Item1;
        public MaterialNodeList Nodes => _level.Item2;
    }
}
