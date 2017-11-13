using Newtonsoft.Json.Linq;

namespace JsonTransformer
{
    internal abstract class Resolver
    {

        public const string Separator = ",";
        public const char OpeningBrace = '{';
        public const char ClosingBrace = '}';
        public const char OpeningRectBracket = '[';
        public const char ClosingRectBracket = ']';
        public const char DotOperator = '.';

        public abstract JToken ProcessJson(string jTokenValue, JToken inputObject);

        public static Resolver ResolveMapper(string token)
        {
            var tokenValue = (char)token?.Trim()[0];
            
            switch (tokenValue)
            {
                case OpeningRectBracket:
                    return new JsonArrayFilterResolver();
                case DotOperator:
                    return new NestedJsonResolver();
                case OpeningBrace:
                    return new JsonSelectResolver();
            }
       
            return new DefaultResolver();
        }
    }
}
