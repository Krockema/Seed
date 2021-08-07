using Seed.Data;
using Seed.Generator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Seed.Data
{
    public class ListExtension<T> : List<T>
    {
        public ListExtension() : base() { }
        private T[] NodeStoreage { get; set; }
        public void SaveNodes()
        {
            NodeStoreage = this.ToArray();
        }

        public T GetNodeAt(int index)
        {
            var node = this[index];
            return node;
        }
        public Node DequeueNode()
        {
            var node = this[0] as Node;
            this.RemoveAt(0);
            Materials.NodesInUse.Add(node);
            return node;
        }
        public int CountAll => NodeStoreage.Length;
        public Node GetNodeFromStorage(int index)
        {
            return NodeStoreage[index] as Node;
        }
    }
}
