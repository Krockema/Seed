using Seed.Generator;
using Seed.Parameter.Operation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;
using static System.Net.Mime.MediaTypeNames;

namespace Seed.Test.DefaultConfiguration
{
    public class ConfigTest
    {
        ITestOutputHelper _out;

        public ConfigTest(ITestOutputHelper helper)
        {
            _out = helper;
        }


        [Theory]
        [InlineData(4, 2, 4)]
        public void ThrowOnInvalidConfigToFewSalesMaterial(int complexity, int reuse, int salesMaterials)
        {
            //Assert.Throws<ArgumentException>(() => new MaterialGenerator(configuration, random));
            //TBD
        }

        [Fact]
        public void ImporterTest()
        {   
            var jsonText = File.ReadAllText(Environment.CurrentDirectory + @"\Config\DefaultResources.json");
            var inJson = System.Text.Json.JsonSerializer.Deserialize<ResourceGroups>(jsonText);
            Assert.NotNull(inJson);
        }

        [Fact]
        public void ExporterTest()
        {
            var rsg = ConfigurationBase.CreateResourceGroups();
            var outJson = System.Text.Json.JsonSerializer.Serialize(rsg);
            _out.WriteLine(outJson);
            File.WriteAllText(Environment.CurrentDirectory + @"\Config\resources.json", outJson);
            Assert.NotNull(outJson);
            Assert.NotEmpty(outJson);
        }

        [Theory]
        [InlineData(1, 1)]
        [InlineData(3, 1)]
        public void TestFallBackFromImport(int resourceIndex, int toolIndex)
        {
            var jsonText = File.ReadAllText(Environment.CurrentDirectory + @"\Config\DefaultResources.json");
            var inJson = System.Text.Json.JsonSerializer.Deserialize<ResourceGroups>(jsonText);
            var meanToolOperationFallback = inJson.GetMeanOperationDurationFor(resourceIndex, toolIndex);
            var varianeToolOperationFallback = inJson.GetVarianceOperationDurationFor(resourceIndex, toolIndex);
            Assert.Equal(TimeSpan.FromSeconds(300), meanToolOperationFallback);
            Assert.Equal(0.2, varianeToolOperationFallback);

        }
    }
}
