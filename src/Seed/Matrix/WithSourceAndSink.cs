namespace Seed.Matrix
{
    public class WithSourceAndSink
    {
        private  bool _value { get; }

        public int Value => _value ? 1 : 0;

        public WithSourceAndSink(bool value)
        {
            _value = value;
        }
    }
}
