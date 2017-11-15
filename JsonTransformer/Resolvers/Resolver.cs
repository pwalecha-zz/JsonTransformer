using Newtonsoft.Json.Linq;

namespace JsonTransformer
{
    internal abstract class Resolver
    {
        protected JProperty property;
        protected Resolver(JProperty prop) => property = prop;

        public const string Separator = ",";
        public const char OpeningBrace = '{';
        public const char ClosingBrace = '}';
        public const char OpeningRectBracket = '[';
        public const char ClosingRectBracket = ']';
        public const char DotOperator = '.';

        public abstract JToken ProcessJson(string jTokenValue, JToken inputObject);

        public static Resolver ResolveMapper(JProperty property, string token)
        {
            var tokenValue = (char)token?.Trim()[0];
            
            switch (tokenValue)
            {
                case OpeningRectBracket:
                    return new JsonArrayFilterResolver(property);
                case DotOperator:
                    return new NestedJsonResolver(property);
                case OpeningBrace:
                    return new JsonSelectResolver(property);
            }
       
            return new DefaultResolver(property);
        }
    }
}
