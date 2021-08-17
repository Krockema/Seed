using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Seed.Data
{
    public class MaterialNodeOperation
    {
        private static int IdCounter = 0;

        public MaterialNodeOperation()
        {
            Id = IdCounter++;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public TimeSpan Duration { get; set;}
        public int SequenceNumber { get; set; }
        public int TargetResourceIdent { get; set;}
        public MaterialNode Node { get; set; }
    }
}
