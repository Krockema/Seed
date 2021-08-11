using Seed.Generator.TraverseActions;

namespace Seed.Generator
{
    public class OperationGenerator : IOperationGenerator
    {
        private IOperationDistributor _operationDistributor;
        private Materials _materials = null;
        public OperationGenerator(IOperationDistributor operationDistributor, Materials materials)
        {
            _operationDistributor = operationDistributor;
            _materials = materials;
        }
        
        public void GenerateOperations()
        {
            
            var traverseAction = new TraverseActionAppendOperations(_operationDistributor);
            var salesNodes = _materials.NodesSalesOnly();

            foreach (var node in salesNodes)
            {
                _materials.Traverse(node.IncomingEdges.ToArray(), traverseAction);
            }
        }
    }
}
