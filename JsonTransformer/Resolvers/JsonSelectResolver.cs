using JsonTransformer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace JsonTransformer
{
    internal class JsonSelectResolver : Resolver
    {
        public override JToken ProcessJson(string jTokenValue, JToken inputObject)
        {
            if (inputObject != null)
            {
                var inputArray = inputObject as JArray;
                JArray outputArray = new JArray();
                if (inputArray is null)
                {
                    return jTokenValue.Contains(Separator) ? GetComplexObject(inputObject, jTokenValue) : inputObject[jTokenValue];
                }

                return GetSelectTokens(inputArray, jTokenValue);
            }

            return null;
        }

        private string GetSelectKey(string token, bool value = false)
        {
            var splitValues = token.Split(new char[] { '=' });
            if (value && splitValues.Length == 2)
                return splitValues[1];

            return splitValues.Length == 2 ? splitValues[0] : token;
        }

        private JObject GetComplexObject(JToken input, string selectAttribute)
        {
            JObject output = new JObject();
            string[] splitSelectValues = selectAttribute.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            if (splitSelectValues.Length > 0)
            {
                foreach (var val in splitSelectValues)
                {
                    output[GetSelectKey(val.Trim())] = input[GetSelectKey(val.Trim(),true).Trim()];
                }
            }
            else
            {
                output[GetSelectKey(selectAttribute.Trim())] = input[GetSelectKey(selectAttribute.Trim(), true)];
            }

            return output;
        }

        private string GetTransformedName(string selectToken)
        {
            var splitValues = selectToken.Split(new char[] { '=' });
            return splitValues.Length == 2 ? splitValues[1] : selectToken;
        }

        private JArray GetSelectTokens(JArray inputArray, string selectAttribute)
        {
            JArray outputArray = new JArray();
            foreach (var row in inputArray)
            {
                outputArray.Add(GetComplexObject(row, selectAttribute));
            }

            return outputArray;
        }

    }
}
