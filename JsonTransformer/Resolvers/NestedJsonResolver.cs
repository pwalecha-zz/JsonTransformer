using Newtonsoft.Json.Linq;

namespace JsonTransformer
{
    internal class NestedJsonResolver : Resolver
    {
        public override JToken ProcessJson(string jTokenValue, JToken inputObject)
        {
            return inputObject[jTokenValue];
        }

    }
}
