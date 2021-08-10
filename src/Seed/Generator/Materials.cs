using Seed.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Seed.Generator
{
    public class Materials : List<Hirachie>
    {
        public static List<Node> NodesInUse { get; set; } = new List<Node>();

        public int CountDequeuedNodesFor(int level) => NodesInUse.Count(x => x.InitialLevel == level);
        /// <summary>
        /// Returns one node from NodeInUse store.
        /// </summary>
        /// <param name="index"></param>
        /// <returns>Node at Index</returns>
        public Node GetNodeInUseBy(int index)
        {
            return NodesInUse[index];
        }
        public Node GetNodeInUseBy(int level, int index)
        {
            return NodesInUse.Where(x => x.InitialLevel == level)
                             .ElementAt(index);
        }
        public int CountNodesInUse => NodesInUse.Count();

        public Node[] ToNodeArray => NodesInUse.ToArray();
 
        public Node[] NodesWithoutPurchase()
        {
            return NodesInUse.Where(x => x.InitialLevel != this.Count - 1).ToArray();
        }
        public Node[] NodesWithoutSales()
        {
            return NodesInUse.Where(x => x.InitialLevel != 0).ToArray();
        }
        public Node[] NodesSalesOnly()
        {
            return NodesInUse.Where(x => x.InitialLevel == 0).ToArray();
        }
        public Node[] NodesPurchaseOnly()
        {
            return NodesInUse.Where(x => x.InitialLevel == this.Count - 1).ToArray();
        }
    }
}
