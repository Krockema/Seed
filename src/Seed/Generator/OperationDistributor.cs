using Seed.Data;
using Seed.Distributions;
using Seed.Matrix;
using Seed.Parameter.Operation;
using System;
using System.Linq;

namespace Seed.Generator
{
    public class OperationDistributor : IOperationDistributor
    {
        private RandomizerCollection _randomizerCollection;
        private ResourceGroups ResourceGroups;
        public OperationDistributor(TransitionMatrix matrix, RandomizerCollection randomizerCollection, ResourceGroups resourceGroups)
        {

            _randomizerCollection = randomizerCollection;
            _matrix = matrix;
            _matrix.TransformToProbability();
        }

        private TransitionMatrix _matrix { get; set;}
        private int Source { get; set; }
        /// <summary>
        /// Creates a new Operation and stores it in Materials.Operations
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public MaterialNodeOperation[] GenerateOperationsFor(MaterialNode node)
        {
            Source = 0; // or Dynamic?

            foreach (var SequenceNumber in Enumerable.Range(1, _randomizerCollection.OperationAmount.Next(_matrix.GetMatrix.RowCount)))
            {
                var targetResourceIndex = _matrix.GetJump(Source, _randomizerCollection.OperationTransition.Next());
                var toolIndex = _randomizerCollection.OperationTransition.Next(ResourceGroups.GetToolsFor(targetResourceIndex).Count);

                // 
                var averageDuration = ResourceGroups.GetAverageOperationDurationFor(targetResourceIndex, toolIndex).TotalSeconds;
                var varianceDuration = ResourceGroups.GetVarianceOperationDurationFor(targetResourceIndex, toolIndex);
                
                var customDuration = TimeSpan.FromSeconds(_randomizerCollection.OperationDuration.NextWithMeanAndVariance(averageDuration, averageDuration * (1 + varianceDuration)));

                var operation = new MaterialNodeOperation { Guid = Guid.NewGuid(), Name = "Operation " + SequenceNumber, Node = node, TargetResourceIdent = Source, Duration = customDuration };
                node.Operations.Add(operation);

                Materials.Operations.Add(operation);

            }
            
            return node.Operations.ToArray();
        }
    }
}
