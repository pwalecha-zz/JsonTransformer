using Newtonsoft.Json.Linq;

namespace JsonTransformer
{
    internal class NestedJsonResolver : Resolver
    {
        internal NestedJsonResolver(JProperty property) : base(property) { }
        public override JToken ProcessJson(string jTokenValue, JToken inputObject)
        {
            return inputObject[jTokenValue];
        }

    }
}
