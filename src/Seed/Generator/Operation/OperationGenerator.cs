using Seed.Data;
using Seed.Matrix;

namespace Seed.Generator.Operation
{
    public class OperationGenerator : IOperationGenerator, IWithOperationDistributor
    {
        private IOperationDistributor _operationDistributor;
        private MaterialNode[] _materials = null;
        private OperationGenerator(IOperationDistributor operationDistributor)
        {
            _operationDistributor = operationDistributor;
        }

        public static IWithOperationDistributor WithOperationDistributor(IOperationDistributor operationDistributor)
        {
            var operationGenerator = new OperationGenerator(operationDistributor);
            return operationGenerator;
        }

        public IOperationGenerator WithMaterials(MaterialNode[] materialsWithoutPurchase)
        {
           this._materials = materialsWithoutPurchase;
           return this;
        }

        public OperationGenerator Generate()
        {
            foreach (var node in _materials)
            {
                _operationDistributor.GenerateOperationsFor(node);
            }
            return this;
        }
        public TransitionMatrix GetGeneratedTransitionMatrix()
        {
            return new TransitionMatrix(_operationDistributor.TargetTransitions);
        }
    }
}
