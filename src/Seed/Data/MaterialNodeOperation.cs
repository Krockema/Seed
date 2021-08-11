using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Seed.Data
{
    public class MaterialNodeOperation
    {
        public Guid Guid{ get; set; }
        public string Name { get; set; }
        public TimeSpan Duration { get; set;}
        public int SequenceNumber { get; set; }
        public int TargetResourceIdent { get; set;}
        public MaterialNode Node { get; set; }
    }
}
