using Newtonsoft.Json.Linq;

namespace JsonTransformer
{
    internal class DefaultResolver : Resolver
    {
        public override JToken ProcessJson(string jTokenValue, JToken inputObject)
        {
            return inputObject[jTokenValue];
        }

    }
}
