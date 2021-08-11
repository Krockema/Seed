using Seed.Data;
using Seed.Generator.TraverseActions;
using System.Collections.Generic;
using System.Linq;

namespace Seed.Generator
{
    public class Materials : List<MaterialHirachie>
    {
        public static List<MaterialNode> NodesInUse { get; set; } = new List<MaterialNode>();
        public static List<MaterialNodeOperation> Operations { get; set; } = new List<MaterialNodeOperation>();

        public int CountDequeuedNodesFor(int level) => NodesInUse.Count(x => x.InitialLevel == level);
        /// <summary>
        /// Returns one node from NodeInUse store.
        /// </summary>
        /// <param name="index"></param>
        /// <returns>Node at Index</returns>
        public MaterialNode GetNodeInUseBy(int index)
        {
            return NodesInUse[index];
        }
        public MaterialNode GetNodeInUseBy(int level, int index)
        {
            return NodesInUse.Where(x => x.InitialLevel == level)
                             .ElementAt(index);
        }
        public int CountNodesInUse => NodesInUse.Count();

        public MaterialNode[] ToNodeArray => NodesInUse.ToArray();
 
        public MaterialNode[] NodesWithoutPurchase()
        {
            return NodesInUse.Where(x => x.InitialLevel != this.Count - 1).ToArray();
        }
        public MaterialNode[] NodesWithoutSales()
        {
            return NodesInUse.Where(x => x.InitialLevel != 0).ToArray();
        }
        public MaterialNode[] NodesSalesOnly()
        {
            return NodesInUse.Where(x => x.InitialLevel == 0).ToArray();
        }
        public MaterialNode[] NodesPurchaseOnly()
        {
            return NodesInUse.Where(x => x.InitialLevel == this.Count - 1).ToArray();
        }

        public void Traverse(MaterialEdge[] edges, ITraverseAction action)
        {
            foreach (var edge in edges)
            {
                action.Do(edge);
                Traverse(edge.From.IncomingEdges.ToArray(), action);
            }
        }
    }
}
