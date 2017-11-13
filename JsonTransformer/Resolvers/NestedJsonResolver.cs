using JsonTransformer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
