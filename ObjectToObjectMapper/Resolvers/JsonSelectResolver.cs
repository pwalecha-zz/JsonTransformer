using JsonTransformer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace JsonTransformer
{
    public class JsonSelectResolver : Resolver
    {
        public override JToken ProcessJson(string jTokenValue, JToken inputObject)
        {
            if (jTokenValue.Contains(Separator)) {
                var inputArray = inputObject as JArray;
                JArray outputArray = new JArray();
                foreach (var row in inputArray)
                {
                    JObject input = new JObject();
                    string[] splitSelectValues = jTokenValue.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var val in splitSelectValues)
                    {
                        input[val.Trim()] = row[val.Trim()];
                    }

                    outputArray.Add(input);
                }

                return outputArray as JToken;
            }
            else
                return inputObject[jTokenValue];
        }

    }
}
