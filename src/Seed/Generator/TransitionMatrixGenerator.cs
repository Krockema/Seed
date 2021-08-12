using Seed.Distributions;
using Seed.Matrix;
using Seed.Parameter;
using Seed.Parameter.TransitionMatrix;
using System;

namespace Seed.Generator
{
    public class TransitionMatrixGenerator
    { 
        public MatrixSize Size { get; set; }
        public Lambda Lambda { get; set; }
        
        public OrganizationalDegree OrganizationalDegree{ get; set; }
        public IRandomizer Randomizer { get; }
        public TransitionMatrix TransitionMatrix { get; set; }

        public TransitionMatrixGenerator(Configuration config, IRandomizer randomizer)
        {
            Size = config.Get<MatrixSize>();
            OrganizationalDegree = config.Get<OrganizationalDegree>();
            Lambda = config.Get<Lambda>();
            Randomizer = randomizer;
        }


        public TransitionMatrix Generate()
        {

            // Initialization Matrix A 

            TransitionMatrix = new TransitionMatrix(Size, new MatrixPoissonInitializer(Lambda));
            //TransitionMatrix.Randomize(Randomizer);
            TransitionMatrix.Normalize();

           
            // Initialization Matrix B depending on organizational Degree of Matrix A
            var matrixB = (TransitionMatrix.GetOrganizationalDegree() > OrganizationalDegree.Value)
                ? new TransitionMatrix(Size, new MatrixNormalInitializer(Size.Value))
                : new TransitionMatrix(Size, new MatrixLeftShiftIdentityInitializer(Size.Value));

            while (Math.Abs(TransitionMatrix.GetOrganizationalDegree()-OrganizationalDegree.Value) > 0.001)
            {
                //matrixA
                var matrixC = TransitionMatrix.HalfAdd(matrixB);

                if (Equals(GetPreSign(TransitionMatrix), GetPreSign(matrixC)))
                {
                    TransitionMatrix.SetMatrix(matrixC.GetMatrix);
                }
                else
                {
                    matrixB.SetMatrix(matrixC.GetMatrix);
                }
            }
            return TransitionMatrix;

        }

        /// <summary>
        /// returns fals if result of the difference to the estimated Operational degree is negative
        /// </summary>
        /// <param name="matrix"></param>
        /// <returns></returns>
        private bool GetPreSign(TransitionMatrix matrix)
        {
            return matrix.GetOrganizationalDegree() - OrganizationalDegree.Value > 0;
        }
    }
}