using JsonTransformer;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObjectToObjectMapper
{
    class Program
    {
        static void Main(string[] args)
        {
            string input = File.ReadAllText(@"c:\users\puwalech\Source\Repos\JsonTranformertest\JsonTranformertest\Input.json");
            string mapper = File.ReadAllText(@"c:\users\puwalech\Source\Repos\JsonTranformertest\JsonTranformertest\Mapper.json");
            JsonMapper transformer = new JsonMapper();
            JObject inputObject = JObject.Parse(input);
            JObject mapperObject = JObject.Parse(mapper);
            var transformedObject = transformer.Transform(inputObject, mapperObject.Root);
            var json = JsonConvert.SerializeObject(transformedObject);
            Console.Read();

        }

    }
}
