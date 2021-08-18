using System.Text.Json.Serialization;

namespace Seed.Data
{
    public class MaterialEdge
    {
        [JsonIgnore]
        private static int IdCounter = 0;
        public MaterialEdge()
        {
            Id = IdCounter++;
        }
        public int Id { get; set;}
        private MaterialNode _from;
        private MaterialNode _to;
        [JsonIgnore]
        public MaterialNode From 
        { 
            get { return _from; }
            set { _from = value;
                  _from.OutgoingEdges.Add(this); } 
        }
        [JsonIgnore]
        public MaterialNode To 
        { 
            get { return _to; } 
            set { _to = value;
                  _to.IncomingEdges.Add(this); } 
            }

        public int FromId => _from.Id;
        public int ToId => _to.Id;
    }
}
