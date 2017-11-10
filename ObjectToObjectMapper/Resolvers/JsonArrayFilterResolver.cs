using JsonTransformer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace JsonTransformer
{
    public class JsonArrayFilterResolver : Resolver
    {
        public override JToken ProcessJson(string jTokenValue, JToken inputObject)
        {
            string[] conditions = jTokenValue.Split(new string[] { Separator }, StringSplitOptions.RemoveEmptyEntries);
            IEnumerable<JToken> tokens = null;
            foreach(var condition in conditions)
            {
                string[] splitKeyValues = condition.Split(new char[] { '=' });
                tokens = (inputObject as JArray).Where(x => x.Value<string>(splitKeyValues[0].Trim()) == splitKeyValues[1].Trim().Replace("'", string.Empty));
            }

            var jArray = new JArray(tokens);
            return jArray.Count() == 1 ? jArray[0] : jArray;

        }
    }
}
