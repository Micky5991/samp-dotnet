using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Micky5991.Samp.Net.Generators.Data
{
    public class IdlAttribute : Dictionary<string, string>
    {
        private static readonly Regex KeyValueExpression = new Regex(@"(?<key>[A-Za-z0-9\-_]+)(?:\((?<value>[^)]+)\))?", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public IdlAttribute(string content)
            : base(BuildData(content))
        {
        }

        private static IDictionary<string, string> BuildData(string content)
        {
            var matches = KeyValueExpression.Matches(content);
            var result = new Dictionary<string, string>(matches.Count);

            for (var i = 0; i < matches.Count; i++)
            {
                var key = matches[i].Groups["key"];
                var value = matches[i].Groups["value"];

                result[key.Value] = value.Success ? value.Value : "1";
            }

            return result;
        }
    }
}
