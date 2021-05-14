using System;
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
            config.AddOption(new MatrixSize(4));
            config.AddOption(new OrganizationalDegree(0.5));
            config.AddOption(new Lambda(5));
            IRandomizer rnd = new Randomizer(1337);

            var generator = new TransitionMatrixGenerator(config, rnd);
            generator.Generate();

            System.Diagnostics.Debug.WriteLine("Achived OrganizationalDegree: " +  generator.TransitionMatrix.GetOrganizationalDegree());
            System.Diagnostics.Debug.Write(generator.TransitionMatrix.GetMatrix.ToString());
            
        }
    }
}
