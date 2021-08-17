using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Seed.Parameter.TransitionMatrix
{
    public class TransitionMatrixParameter : IParameter
    {
        public TransitionMatrixParameter() { }
    
        public double Lambda {  get; set; }
        public double OrganizationalDegree {  get; set; }
    }
}
