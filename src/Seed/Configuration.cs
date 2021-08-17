using Seed.Parameter;
using Seed.Parameter.Operation;

namespace Seed
{
    public class Configuration : Dictionary<string, IParameter>
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

        public static T ReadFromFile<T>(string fileName)
        {
            var jsonText = File.ReadAllText(Environment.CurrentDirectory + $@"\Config\{fileName}");
            var inJson = System.Text.Json.JsonSerializer.Deserialize<T>(jsonText);
            return inJson;
        }

        public Configuration WithOption(IParameter o)
        {
            if (this.TryAdd(key: o.GetType().Name, value: o))
                return this;
            
            throw new ArgumentException($"Option {o.GetType().Name} already added");
        }

        public T Get<T>()
        {
            if (this.TryGetValue(key: typeof(T).Name, value: out IParameter value))
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
