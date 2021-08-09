using Seed.Distributions;
using Seed.Generator;
using Seed.Matrix;
using Seed.Parameter;
using Seed.Parameter.TransitionMatrix;
using Xunit;

namespace Seed.Test
{
    public class MatrixTest
    {
        [Fact]
        public void IsUniform()
        {
            var matrix = new TransitionMatrix(new MatrixSize(4), new MatrixIdentityInitializer());
            var degree = matrix.GetOrganizationalDegree(); 
            Assert.Equal(1, degree);
        }

        [Fact]
        public void IsNormal()
        {
            var matrix = new TransitionMatrix(new MatrixSize(4), new MatrixNormalInitializer(4));
            var degree = matrix.GetOrganizationalDegree(); 
            Assert.Equal(0, degree);
        }

        [Fact]
        public void Transition()
        {
            var config = new Configuration();
            config.WithOption(new MatrixSize(4));
            config.WithOption(new OrganizationalDegree(0.15));
            config.WithOption(new Lambda(0.7));
            IRandomizer rnd = new Randomizer(1337);

            var generator = new TransitionMatrixGenerator(config, rnd);
            generator.Generate();

            System.Diagnostics.Debug.WriteLine("Achieved OrganizationalDegree: " +  generator.TransitionMatrix.GetOrganizationalDegree());
            System.Diagnostics.Debug.Write(generator.TransitionMatrix.GetMatrix.ToString());
            
        }

        [Fact]
        public void IsAscending()
        {
            var size = 5;
            var matrix = new ProbabilityByDistanceMatrix(new MatrixSize(size), new MatrixProbabilityByDistanceInitializerAscending(size));
            System.Diagnostics.Debug.Write(matrix.GetMatrix.ToString());
            Assert.Equal(4, matrix.GetRowJump(2, 0.55));
            Assert.Equal(3, matrix.GetRowJump(2, 0.4));
        }
        [Fact]
        public void IsDescending()
        {
            var matrix = new ProbabilityByDistanceMatrix(new MatrixSize(10), new MatrixProbabilityByDistanceInitializerDescending());
            System.Diagnostics.Debug.Write(matrix.GetMatrix.ToString());
            System.Diagnostics.Debug.Write(matrix.GetValuesFor(9).ToString());
            var targetRow = matrix.GetRowJump(9, 0.2);
            System.Diagnostics.Debug.WriteLine("Get row jump " + targetRow);
            Assert.Equal(3, targetRow);
            Assert.Equal(0, matrix.GetRowJump(1, 0.2));
        }
    }
}
