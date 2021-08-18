using Seed.Data;
using System.Collections.Generic;

namespace Seed.Generator.Material
{
    public interface IWithOperationsInUse
    {
        public List<MaterialNodeOperation> Operations { get; }
    }
}
