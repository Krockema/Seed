
using Seed.Generator;

using System.Collections.Generic;

namespace Seed.Data
{
    public class NodeList: List<Node>
    {
        public NodeList() : base() { }
        private Node[] NodeStoreage { get; set; }
        public void SaveNodes()
        {
            NodeStoreage = this.ToArray();
        }

        public Node GetNodeAt(int index)
        {
            return this[index];
        }
        public Node DequeueNode()
        {
            var node = this[0];
            this.RemoveAt(0);
            Materials.NodesInUse.Add(node);
            return node;
        }
        public int CountAll => NodeStoreage.Length;
        public Node GetNodeFromStorage(int index)
        {
            return NodeStoreage[index];
        }
    }
}
