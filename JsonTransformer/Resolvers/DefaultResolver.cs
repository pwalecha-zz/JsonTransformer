using Newtonsoft.Json.Linq;

namespace JsonTransformer
{
    internal class DefaultResolver : Resolver
    {
        internal DefaultResolver(JProperty property) : base(property) { }
        public override JToken ProcessJson(string jTokenValue, JToken inputObject)
        {
            return inputObject[jTokenValue];
        }

    }
}
