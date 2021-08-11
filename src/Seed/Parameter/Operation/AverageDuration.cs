using System;

namespace Seed.Parameter.Material
{
    public class AverageDuration : Option<TimeSpan>
    {
        public AverageDuration(TimeSpan duration) : base(duration) {}
    }
}
