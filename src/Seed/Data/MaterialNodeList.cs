﻿
using Seed.Generator;
using Seed.Generator.Material;
using System.Collections.Generic;

namespace Seed.Data
{
    public class MaterialNodeList: List<MaterialNode>
    {
        IWithNodeInUse _nodeCollector;
        public MaterialNodeList(IWithNodeInUse nodeCollector) : base() {
            _nodeCollector = nodeCollector;
        }
        private MaterialNode[] NodeStoreage { get; set; }
        public void SaveNodes()
        {
            NodeStoreage = this.ToArray();
        }

        public MaterialNode GetNodeAt(int index)
        {
            return this[index];
        }
        public MaterialNode DequeueNode()
        {
            var node = this[0];
            this.RemoveAt(0);
            _nodeCollector.NodesInUse.Add(node);
            return node;
        }
        public bool DequeueNode(MaterialNode node)
        {
            var suc =  this.Remove(node);
            _nodeCollector.NodesInUse.Add(node);
            return suc;
        }

        public int CountAll => NodeStoreage.Length;
        public MaterialNode GetNodeFromStorage(int index)
        {
            return NodeStoreage[index];
        }
    }
}
