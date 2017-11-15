using Newtonsoft.Json.Linq;

namespace ObjectToObjectMapper
{
    interface ITransform
    {
        JToken Transform(JToken input, JToken mapper);
    }
}
