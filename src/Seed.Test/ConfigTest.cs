using Seed.Generator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Seed.Test
{
    public class ConfigTest
    {
        [Theory]
        [InlineData(4, 2, 4)]
        public void ThrowOnInvalidConfigToFewSalesMaterial(int complexity, int reuse, int salesMaterials)
        {
            //Assert.Throws<ArgumentException>(() => new MaterialGenerator(configuration, random));
            //TBD
        }
    }
}
