using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Seed.Parameter.Operation
{
    public class VarianceDuration : Option<double>
    {
        public VarianceDuration(double duration) : base(duration) { }
    }
}
