
using Seed.Generator;

using System.Collections.Generic;

namespace Seed.Data
{
    public class MaterialNodeList: List<MaterialNode>
    {
        public MaterialNodeList() : base() { }
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
            Materials.NodesInUse.Add(node);
            return node;
        }
        public int CountAll => NodeStoreage.Length;
        public MaterialNode GetNodeFromStorage(int index)
        {
            return NodeStoreage[index];
        }
    }
}
