using Seed.Data;
using Seed.Distributions;
using Seed.Generator.Material;
using Seed.Matrix;
using Seed.Parameter;

namespace Seed.Generator.Operation
{
    public class OperationDistributor : IOperationDistributor, IWithTransitionMatrix, IWithRandomizerCollection, IWithResourceConfig, IWithMaterials
    {
        private RandomizerCollection _randomizerCollection;
        private ResourceConfig _resourceGroups;
        private IWithOperationsInUse _operationsInUse;
        private bool _withSourceAndSink;
        private TransitionMatrix _matrix { get; set; }
        private int Source { get; set; }
        public double[,] TargetTransitions { get; private set; }
        public OperationDistributor(TransitionMatrix matrix)
        {
            _withSourceAndSink = false;
            _matrix = matrix;
            TargetTransitions = new double[matrix.GetMatrix.RowCount, matrix.GetMatrix.ColumnCount];
            _matrix.TransformToProbability();
            Source = 0;
        }

        public static IWithTransitionMatrix WithTransitionMatrix(TransitionMatrix matrix)
        {
            return new OperationDistributor(matrix);
        }

        public IWithRandomizerCollection WithRandomizerCollection(RandomizerCollection randomizerCollection)
        {
            _randomizerCollection = randomizerCollection;
            return this;
        }

        public IWithResourceConfig WithResourceConfig(ResourceConfig resourceConfig)
        {
            _resourceGroups = resourceConfig;
            return this;
        }

        public IWithMaterials WithMaterials(IWithOperationsInUse operationsInUse)
        {
            _operationsInUse = operationsInUse;
            return this;
        }

        public OperationDistributor Build()
        {
            return this;
        }

        /// <summary>
        /// Creates a new Operation and stores it in Materials.Operations
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public MaterialNodeOperation[] GenerateOperationsFor(MaterialNode node)
        {
            var breakOn = _randomizerCollection.OperationAmount
                                                .NextWithMeanAndVariance(_resourceGroups.DefaultOperationAmountDistributionParameter.Mean
                                                                        , _resourceGroups.DefaultOperationAmountDistributionParameter.Variance);
            /* not implemented yet */
            // if (_withSourceAndSink)
            // {
            //     breakOn = int.MaxValue;
            //     Source = 0;
            // }

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

                var operation = new MaterialNodeOperation { Name = "Operation " + node.Operations.Count + 1
                                                            , Node = node
                                                            , TargetResourceIdent = targetResourceIndex
                                                            , TargetToolIdent = toolIndex
                                                            , SequenceNumber = node.Operations.Count + 1
                                                            , Cost = Math.Round(customDuration.TotalHours * _resourceGroups.GetCostRateProcessingFor(targetResourceIndex), 2) 
                                                            , Duration = customDuration };
                node.Operations.Add(operation);

                _operationsInUse.Operations.Add(operation);
                Source = targetResourceIndex;
            }
        }
    }
}
