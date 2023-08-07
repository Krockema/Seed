﻿using Seed.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Seed.Generator.Material
{
    public class Materials : List<MaterialHirachie>, IWithOperationsInUse, IWithNodeInUse
    {
        public HashSet<MaterialNode> NodesInUse { get;  } = new HashSet<MaterialNode>();
        public List<MaterialNodeOperation> Operations { get; } = new List<MaterialNodeOperation>();
        public List<MaterialEdge> Edges {  get; } = new List<MaterialEdge>();
        public int CountDequeuedNodesFor(int level) => NodesInUse.Count(x => x.InitialLevel == level);
        /// <summary>
        /// Returns one node from NodeInUse store.
        /// </summary>
        /// <param name="index"></param>
        /// <returns>Node at Index</returns>
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

        public static void CalculateCosts(MaterialEdge[] edges)
        {
            foreach (var edge in edges)
            {
                CalculateCosts(edge.From.IncomingEdges.ToArray());
                edge.To.Cost = Math.Round(edge.To.Operations.Sum(x => x.Cost) + edge.From.Cost, 2);
            }
        }
    }
}
