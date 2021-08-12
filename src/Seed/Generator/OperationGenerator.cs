using Seed.Data;
using Seed.Generator.TraverseActions;
using Seed.Matrix;

namespace Seed.Generator
{
    public class OperationGenerator : IOperationGenerator
    {
        private IOperationDistributor _operationDistributor;
        private MaterialNode[] _materials = null;
        public OperationGenerator(IOperationDistributor operationDistributor, MaterialNode[] materialsWithoutPurchase)
        {
            _operationDistributor = operationDistributor;
            _materials = materialsWithoutPurchase;
        }
        
        public void GenerateOperations(bool rerollStart)
        {
              foreach (var node in _materials)
            {
                _operationDistributor.GenerateOperationsFor(node, rerollStart);
            }
        }

        public TransitionMatrix GetGeneratedTransitionMatrix()
        {
            return new TransitionMatrix(_operationDistributor.TargetTransitions);
        }
    }
}
