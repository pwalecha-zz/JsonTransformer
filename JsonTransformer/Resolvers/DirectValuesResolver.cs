using System;
using Newtonsoft.Json.Linq;

namespace JsonTransformer
{
    internal class DirectValuesResolver : Resolver
    {
        internal DirectValuesResolver(JProperty property) : base(property) { }
        public override JToken ProcessJson(string jTokenValue, JToken inputObject)
        {
            if (property != null)
            {
                switch (property.Value.Type)
                {
                    case JTokenType.Boolean:
                    case JTokenType.Integer: return property.Value;
                    default:
                        return jTokenValue;
                }
            }
            return jTokenValue;
        }
    }
}
