using Seed.Data;
using System.Collections.Generic;

namespace Seed.Generator.Material
{
    public interface IWithNodeInUse
    {
        public HashSet<MaterialNode> NodesInUse { get; }
    }
}
