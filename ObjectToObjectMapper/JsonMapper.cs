using Newtonsoft.Json.Linq;
using ObjectToObjectMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsonTransformer
{
    public class JsonMapper : ITransform
    {
        public JToken Transform(JToken jObject, JToken targetObjectMapper)
        {
            var transformedObject = targetObjectMapper;
            foreach (var child in transformedObject.Children())
            {
                JProperty prop = child as JProperty;
                if (prop != null)
                {
                    string propValue = Convert.ToString(prop.Value);
                    var transformers = GetListOfResolvers(propValue);
                    foreach (var transformer in transformers)
                    {
                        if (!string.IsNullOrWhiteSpace(transformer.Item1))
                            jObject = transformer.Item2.ProcessJson(transformer.Item1, jObject);
                    }
                }
            }

            return jObject;
        }
        public static List<int> AllIndexOf(string text, string str, StringComparison comparisonType)
        {
            List<int> allIndexOf = new List<int>();
            int index = text.IndexOf(str, comparisonType);
            while (index != -1)
            {
                allIndexOf.Add(index);
                index = text.IndexOf(str, index + str.Length, comparisonType);
            }
            return allIndexOf;
        }

        private List<Tuple<string, Resolver>> GetListOfResolvers(string mappedJsonValues)
        {
            char[] keys = { Resolver.DotOperator, Resolver.OpeningBrace, Resolver.OpeningRectBracket};
            var list = new List<Tuple<string, Resolver>>();
            Dictionary<char, char> dic = new Dictionary<char, char>();
            dic.Add(Resolver.OpeningBrace, Resolver.ClosingBrace);
            dic.Add(Resolver.OpeningRectBracket, Resolver.ClosingRectBracket);

            while(mappedJsonValues.Any( x => keys.Contains(x)))
            {
                var firstIndexofAny = mappedJsonValues.IndexOfAny(keys);
                var charAtMatchingIndex = mappedJsonValues[firstIndexofAny];
                if(list.Count() == 0 && firstIndexofAny > 0)
                {
                    list.Add(new Tuple<string, Resolver>(mappedJsonValues.Substring(0,firstIndexofAny), new DefaultResolver()));
                    mappedJsonValues =  mappedJsonValues.Remove(0, firstIndexofAny);
                }
                else
                {
                    if(charAtMatchingIndex != '.')
                    {
                        var wholevalue = mappedJsonValues.Substring(firstIndexofAny+ 1, mappedJsonValues.IndexOf(dic[charAtMatchingIndex]) -1);
                        list.Add(new Tuple<string, Resolver>(wholevalue, Resolver.ResolveMapper(charAtMatchingIndex.ToString())));
                        mappedJsonValues = mappedJsonValues.Remove(firstIndexofAny, mappedJsonValues.IndexOf(dic[charAtMatchingIndex])+ 1);
                    }
                    else
                    {
                        if (mappedJsonValues.IndexOfAny(keys, firstIndexofAny + 1) == 0) continue;
                        var val = mappedJsonValues.Substring(firstIndexofAny + 1, mappedJsonValues.IndexOfAny(keys, firstIndexofAny + 1) -1);
                        list.Add(new Tuple<string, Resolver>(val, Resolver.ResolveMapper(charAtMatchingIndex.ToString())));
                        mappedJsonValues =mappedJsonValues.Remove(firstIndexofAny, mappedJsonValues.IndexOfAny(keys, firstIndexofAny + 1));
                    }
                }
            }

            return list;
        }
        

        private List<Tuple<string, Resolver, string>> GetListOfAssociatedResolvers(string mappedJsonValue)
        {
            List<Tuple<string, Resolver, string>> list = new List<Tuple<string, Resolver, string>>();
            List<Tuple<string, int, string>> ls = new List<Tuple<string, int, string>>();


            var indexOfDot = AllIndexOf(mappedJsonValue, ".", StringComparison.OrdinalIgnoreCase);
            var indexOfOpeningSqBracket = AllIndexOf(mappedJsonValue, "[", StringComparison.OrdinalIgnoreCase);
            var indexOfBrace = AllIndexOf(mappedJsonValue, "{", StringComparison.OrdinalIgnoreCase);
            var indexOfSelectorArray = AllIndexOf(mappedJsonValue, "[{", StringComparison.OrdinalIgnoreCase);

            var indexOfClosingSqBracket = AllIndexOf(mappedJsonValue, "]", StringComparison.OrdinalIgnoreCase);

            if (indexOfDot?.Count() > 0)
            {
                indexOfDot.ForEach(x => ls.Add(new Tuple<string, int, string>(".", x, ".")));
            }

            if (indexOfOpeningSqBracket?.Count() > 0)
            {
                indexOfOpeningSqBracket.ForEach(x => ls.Add(new Tuple<string, int, string>("[", x, "]")));
            }

            if (indexOfClosingSqBracket?.Count() > 0)
            {
                indexOfClosingSqBracket.ForEach(x => ls.Add(new Tuple<string, int, string>("]", x, "[")));
            }

            if (indexOfBrace?.Count() > 0)
            {
                indexOfBrace.ForEach(x => ls.Add(new Tuple<string, int, string>("{", x, "}")));
            }

            if (indexOfSelectorArray?.Count() > 0)
            {
                indexOfSelectorArray.ForEach(x => ls.Add(new Tuple<string, int, string>("[{", x, "}]")));
            }

            var orderList = ls.OrderBy(x => x.Item2);

            if (orderList.First().Item2 > 0)
            {
                list.Add(new Tuple<string, Resolver, string>(mappedJsonValue.Substring(0, orderList.First().Item2), new DefaultResolver(), ""));
            }

            foreach (var keyValue in orderList)
            {
                string val;
                if (keyValue.Item1 != ".")
                {
                    var count = list.Count(x => x.Item1 == keyValue.Item1);
                    var lastPositionOfSame = orderList.Where(x => x.Item1 == keyValue.Item1).Select(x => x.Item2).ToList();
                    var endingLength = mappedJsonValue.IndexOf(keyValue.Item3, lastPositionOfSame[count] + 1);
                    int length = endingLength - keyValue.Item2;
                    val = mappedJsonValue.Substring(keyValue.Item2 + 1, length - 1);
                }
                else
                {
                    var immediateNextKey = mappedJsonValue.IndexOfAny(new char[] { '{', '[', '.' }, keyValue.Item2 + 1);
                    val = mappedJsonValue.Substring(keyValue.Item2 + 1, immediateNextKey - keyValue.Item2 - 1);
                }

                list.Add(new Tuple<string, Resolver, string>(val, Resolver.ResolveMapper(keyValue.Item1), keyValue.Item3));

            }

            return list;
        }

    }
}

