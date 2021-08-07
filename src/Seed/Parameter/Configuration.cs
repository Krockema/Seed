using System;
using System.Collections.Generic;

namespace Seed.Parameter
{
    public class Configuration : Dictionary<Type, object>
    {
        public static Configuration Create(object[] args)
        {
            var s = new Configuration();
            foreach (var item in args)
            {
                s.WithOption(o: item);
            }

            return s;
        }

        public Configuration WithOption(object o)
        {
            if (this.TryAdd(key: o.GetType(), value: o))
                return this;
            
            throw new ArgumentException($"Option {o.GetType().Name} already added");
        }

        public T Get<T>()
        {
            if (this.TryGetValue(key: typeof(T), value: out object value))
                return (T)value;
            
            throw new ArgumentException($"{typeof(T).Name}  Not found");
        }

        public Configuration ReplaceOption(object o)
        {
            this.Remove(o.GetType());
            return WithOption(o);
        }
    }
}
