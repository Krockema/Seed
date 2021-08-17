using Seed.Data;
using System.Collections.Generic;

namespace Seed.Generator
{
    public interface IWithNodeInUse
    {
        public List<MaterialNode> NodesInUse { get; }
    }
}
