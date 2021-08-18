using Seed.Parameter;
using Seed.Parameter.Operation;
using System;
using System.IO;
using System.Text.Json;
using Xunit;
using Xunit.Abstractions;

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
            var jsonText = File.ReadAllText(Path.Combine(Environment.CurrentDirectory, @"Config", @"ResourceConfig.json"));
            var inJson = System.Text.Json.JsonSerializer.Deserialize<ResourceConfig>(jsonText);
            Assert.NotNull(inJson);
        }

        [Fact]
        public void ExporterTest()
        {
            var rsg = ConfigurationBase.CreateResourceConfig();
            var outJson = JsonSerializer.Serialize(rsg);
            _out.WriteLine(outJson);
            File.WriteAllText(Path.Combine(Environment.CurrentDirectory, @"Config", @"resources.json"), outJson);
            Assert.NotNull(outJson);
            Assert.NotEmpty(outJson);
            Assert.True(File.Exists(Path.Combine(Environment.CurrentDirectory, @"Config", @"resources.json")));
        }

        [Theory]
        [InlineData(1, 1)]
        [InlineData(3, 1)]
        public void TestFallBackFromImport(int resourceIndex, int toolIndex)
        {
            var resourceGroups = Configuration.ReadFromFile<ResourceConfig>("ResourceConfig.json");
            var meanToolOperationFallback = resourceGroups.GetMeanOperationDurationFor(resourceIndex, toolIndex);
            var varianeToolOperationFallback = resourceGroups.GetVarianceOperationDurationFor(resourceIndex, toolIndex);
            Assert.Equal(TimeSpan.FromSeconds(300), meanToolOperationFallback);
            Assert.Equal(0.2, varianeToolOperationFallback);

            Assert.Equal(100.0, resourceGroups.GetCostRateSetupFor(resourceIndex));
            Assert.Equal(60.0, resourceGroups.GetCostRateProcessingFor(resourceIndex));
            Assert.Equal(10.0, resourceGroups.GetCostRateIdleTimeFor(resourceIndex));
        }

        [Fact] 
        public void WriteMaterialConfig()
        {
            var config = ConfigurationBase.Default();
            var outJson = JsonSerializer.Serialize(config.Get<MaterialConfig>(), new JsonSerializerOptions() { WriteIndented = true });
           _out.WriteLine(outJson);
            File.WriteAllText(Path.Combine(Environment.CurrentDirectory, @"Config", @"MaterialConfig.json"), outJson);
            Assert.NotNull(outJson);
            Assert.NotEmpty(outJson);
            Assert.True(File.Exists(Path.Combine(Environment.CurrentDirectory, @"Config", @"MaterialConfig.json")));
        }


        [Fact]
        public void ReadMaterialConfig()
        {
            var inJson = Configuration.ReadFromFile<MaterialConfig>("MaterialConfig.json");
            _out.WriteLine(inJson.ToString());
            Assert.NotNull(inJson);
            Assert.Equal(2, inJson.TransitionMatrixParameter.Lambda);

        }
    }
}
