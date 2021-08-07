namespace Seed.Parameter
{
    public abstract class Option<T> 
    {
        protected Option(T value)
        {
            _value = value;
        }
        protected T _value;
        public T Value => _value;
    }
}