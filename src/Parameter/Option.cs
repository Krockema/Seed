namespace Seed.Parameter
{
    public abstract class Option<T> 
    {
        protected T _value;
        public T Value => _value;
    }
}