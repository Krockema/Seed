using Seed.Parameter;
using System;
using System.Collections.Generic;
using System.IO;

namespace Seed
{
    public class Configuration : Dictionary<string, object>
    {
        public static Configuration Create(IParameter[] args)
        {
            var s = new Configuration();
            foreach (var item in args)
            {
                s.WithOption(o: item);
            }

            return s;
        }

        public static T ReadFromFile<T>(string fileName) where T : IParameter
        {
            var jsonText = File.ReadAllText(Path.Combine(Environment.CurrentDirectory, @"Config", fileName));
            var inJson = System.Text.Json.JsonSerializer.Deserialize<T>(jsonText);
            return inJson;
        }

        public Configuration WithOption(IParameter o)
        {
            if (this.TryAdd(key: o.GetType().Name, value: o))
                return this;
            
            throw new ArgumentException($"Option {o.GetType().Name} already added");
        }

        public T Get<T>() where T : IParameter
        {
            if (this.TryGetValue(key: typeof(T).Name, value: out object value))
                return (T)value;
            
            throw new ArgumentException($"{typeof(T).Name}  Not found");
        }

        public Configuration ReplaceOption(IParameter o)
        {
            this.Remove(o.GetType().Name);
            return WithOption(o);
        }
    }
}
