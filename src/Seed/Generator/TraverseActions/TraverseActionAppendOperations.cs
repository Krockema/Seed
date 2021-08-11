using Seed.Data;

namespace Seed.Generator.TraverseActions
{
    class TraverseActionAppendOperations : ITraverseAction
    {
        private IOperationDistributor OperationDistributor { get; }
        public TraverseActionAppendOperations(IOperationDistributor operationDistributor)
        {
            OperationDistributor = operationDistributor;
        }
        
        public void Do(MaterialEdge edge, params object[] options)
        {
           var node = edge.To;
           OperationDistributor.GenerateOperationsFor(node);
        }
    }
}
