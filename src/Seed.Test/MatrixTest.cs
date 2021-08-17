using Seed.Distributions;
using Seed.Generator;
using Seed.Matrix;
using Seed.Parameter;
using Seed.Parameter.TransitionMatrix;
using Xunit;
using Xunit.Abstractions;

namespace Seed.Test
{
    public class MatrixTest
    {
        ITestOutputHelper _out;
        public MatrixTest(ITestOutputHelper helper)
        { 
            _out = helper;
        }

        [Fact]
        public void IsUniform()
        {
            var size = 4;
            var matrix = new TransitionMatrix(size, new MatrixLeftShiftIdentityInitializer(size));
            var degree = matrix.GetOrganizationalDegree(); 
            _out.WriteLine(matrix.GetMatrix.ToString());
            Assert.Equal(1, degree);
        }

        [Fact]
        public void IsNormal()
        {
            var matrix = new TransitionMatrix(4, new MatrixNormalInitializer(4));
            var degree = matrix.GetOrganizationalDegree(); 
            Assert.Equal(0, degree);
        }

        [Fact]
        public void Transition()
        {
            var config = new Configuration();
            var transition = new TransitionMatrixParameter() { Lambda = 2, OrganizationalDegree = 0.15 };
            config.WithOption(transition);
            config.WithOption(Configuration.ReadFromFile<ResourceGroupParameter>("ExsampleResources.json"));

            var generator = new TransitionMatrixGenerator(config);
            generator.Generate();

            _out.WriteLine("Achieved OrganizationalDegree: " +  generator.TransitionMatrix.GetOrganizationalDegree());
            _out.WriteLine(generator.TransitionMatrix.GetMatrix.ToString());
            
        }

        [Fact]
        public void IsAscending()
        {
            var size = 5;
            var matrix = new ProbabilityByDistanceMatrix(size, new MatrixProbabilityByDistanceInitializerAscending(size));
            _out.WriteLine(matrix.GetMatrix.ToString());
            Assert.Equal(4, matrix.GetRowJump(2, 0.55));
            Assert.Equal(3, matrix.GetRowJump(2, 0.4));
        }
        [Fact]
        public void IsDescending()
        {
            var matrix = new ProbabilityByDistanceMatrix(10, new MatrixProbabilityByDistanceInitializerDescending());
            _out.WriteLine(matrix.GetMatrix.ToString());
            _out.WriteLine(matrix.GetValuesFor(9).ToString());
            var targetRow = matrix.GetRowJump(9, 0.2);
            _out.WriteLine("Get row jump " + targetRow);
            Assert.Equal(3, targetRow);
            Assert.Equal(0, matrix.GetRowJump(1, 0.2));
        }
    }
}
