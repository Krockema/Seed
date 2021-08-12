using Seed.Data;
using Seed.Distributions;
using Seed.Matrix;
using Seed.Parameter.Operation;
using System;

namespace Seed.Generator
{
    public class OperationDistributor : IOperationDistributor
    {
        private RandomizerCollection _randomizerCollection;
        private ResourceGroups _resourceGroups;
        private bool _withSourceAndSink;
        private TransitionMatrix _matrix { get; set; }
        private int Source { get; set; }
        public double[,] TargetTransitions { get; private set; }

        public OperationDistributor(TransitionMatrix matrix, RandomizerCollection randomizerCollection, ResourceGroups resourceGroups)
        {
            _withSourceAndSink = false;
            _randomizerCollection = randomizerCollection;
            _resourceGroups = resourceGroups;
            _matrix = matrix;
            TargetTransitions = new double[matrix.GetMatrix.RowCount, matrix.GetMatrix.ColumnCount];
            _matrix.TransformToProbability();
            Source = 0;
        }

        /// <summary>
        /// Creates a new Operation and stores it in Materials.Operations
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public MaterialNodeOperation[] GenerateOperationsFor(MaterialNode node, bool rerollStart)
        {
            if (rerollStart)
                Source = _randomizerCollection.OperationTransition.Next(_matrix.GetMatrix.RowCount); // reroll start case

            var breakOn = _randomizerCollection.OperationAmount
                                                .NextWithMeanAndVariance(_resourceGroups.DefaultOperationAmountDistributionParameter.Mean
                                                                        , _resourceGroups.DefaultOperationAmountDistributionParameter.Variance);

            if (_withSourceAndSink)
            {
                breakOn = int.MaxValue;
                Source = 0;
            }

            while (true) {
                var targetResourceIndex = _matrix.GetJump(Source, _randomizerCollection.OperationTransition.Next());
                if ((_withSourceAndSink && targetResourceIndex == _matrix.GetMatrix.RowCount - 1) // with source and sink OR
                    || (breakOn == node.Operations.Count))                                       // with average sequence number
                    return node.Operations.ToArray();

                TargetTransitions[Source, targetResourceIndex]++;

                var toolIndex = _randomizerCollection.OperationTransition.Next(_resourceGroups.GetToolsFor(targetResourceIndex).Count);

                var averageDuration = _resourceGroups.GetMeanOperationDurationFor(targetResourceIndex, toolIndex).TotalSeconds;
                var varianceDuration = _resourceGroups.GetVarianceOperationDurationFor(targetResourceIndex, toolIndex); // as percent
                
                var customDuration = TimeSpan.FromSeconds(_randomizerCollection.OperationDuration.NextWithMeanAndVariance(averageDuration, averageDuration * varianceDuration));

                var operation = new MaterialNodeOperation { Guid = Guid.NewGuid()
                                                            , Name = "Operation " + node.Operations.Count + 1
                                                            , Node = node
                                                            , TargetResourceIdent = Source
                                                            , SequenceNumber = node.Operations.Count + 1
                                                            , Duration = customDuration };
                node.Operations.Add(operation);

                Materials.Operations.Add(operation);
                Source = targetResourceIndex;
            }
        }
    }
}
