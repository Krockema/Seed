using Seed.Distributions;
using Seed.Matrix;
using Seed.Parameter;
using Seed.Parameter.TransitionMatrix;
using System;

namespace Seed.Generator.Operation
{
    public class TransitionMatrixGenerator : IWithTransitionMatrixConfig
    {
        private TransitionMatrixParameter transitionMatrixParameter { get; }
        double OrganizationalDegree => transitionMatrixParameter.OrganizationalDegree;
        double Lambda => transitionMatrixParameter.Lambda;
        int Size { get; } 
        public TransitionMatrix TransitionMatrix { get; set; }

        private TransitionMatrixGenerator(Configuration config)
        {
            transitionMatrixParameter = config.Get<MaterialConfig>().TransitionMatrixParameter;
            Size = config.Get<ResourceConfig>().ResourceGroupList.Count;
        }

        public static IWithTransitionMatrixConfig WithConfiguration(Configuration config)
        {
            return new TransitionMatrixGenerator(config);
        }

        public TransitionMatrix Generate()
        {

            // Initialization Matrix A 

            TransitionMatrix = new TransitionMatrix(Size, new MatrixPoissonInitializer(Lambda));
            //TransitionMatrix.Randomize(Randomizer);
            TransitionMatrix.Normalize();

           
            // Initialization Matrix B depending on organizational Degree of Matrix A
            var matrixB = (TransitionMatrix.GetOrganizationalDegree() > OrganizationalDegree)
                ? new TransitionMatrix(Size, new MatrixNormalInitializer(Size))
                : new TransitionMatrix(Size, new MatrixLeftShiftIdentityInitializer(Size));

            while (Math.Abs(TransitionMatrix.GetOrganizationalDegree()- OrganizationalDegree) > 0.001)
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
            return matrix.GetOrganizationalDegree() - OrganizationalDegree > 0;
        }
    }
}