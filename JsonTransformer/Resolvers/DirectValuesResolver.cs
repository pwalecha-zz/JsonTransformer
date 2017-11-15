using System;
using Newtonsoft.Json.Linq;

namespace JsonTransformer
{
    internal class DirectValuesResolver : Resolver
    {
        public override JToken ProcessJson(string jTokenValue, JToken inputObject)
        {
            return jTokenValue;
        }
    }
}
