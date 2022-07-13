using System.Reflection;
using System.Text.Json;

namespace AutocompleteTypes
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var asm = Assembly.LoadFrom("AutocompleteTypes.dll");
            var types = asm.GetTypes();
            var typesWithAtr = types.Where(t => t.IsDefined(typeof(StubGenAttribute)));

            foreach (var type in typesWithAtr)
            {
                var jsonString = JsonSerializer.Serialize(GenObject(type.Name));
                Console.WriteLine(jsonString);
            }

            Console.ReadLine();
        }

        public static object GenObject(string typeString)
        {
            var asm = Assembly.LoadFrom("AutocompleteTypes.dll");
            var types = asm.GetTypes();
            var type = types.Where(t => t.Name == typeString).FirstOrDefault();

            var props = type.GetProperties();

            if (props.Count() < 1)
            {
                return null;
            }

            var instance = Activator.CreateInstance(type);

            foreach (var item in props)
            {
                switch (item.PropertyType.Name)
                {
                    case "String":
                        item.SetValue(instance, "SomeString");
                        break;
                    case "Int32":
                        item.SetValue(instance, 152);
                        break;
                    case "Decimal":
                        item.SetValue(instance, 150M);
                        break;
                    case "Single":
                        item.SetValue(instance, 150f);
                        break;
                    case "Boolean":
                        item.SetValue(instance, true);
                        break; 
                    default:
                        break;
                }
            }
            return instance;
        }
    }

    [StubGen]
    public class Product
    {
        public int Id { get; set; }
        public decimal Price { get; set; }
        public bool IsBuy { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }

    [StubGen]
    public class Person
    {
        public int Id { get; set; }
        public int Age { get; set; }
        public bool IsDel { get; set; }
        public string Name { get; set; }
    }
}