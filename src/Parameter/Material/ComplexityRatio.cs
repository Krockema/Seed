using MathNet.Numerics;

namespace Seed.Parameter.Material
{
    public class ComplexityRatio: Option<int>
    {
        public ComplexityRatio(int ratio) : base(ratio) {}
    }
}