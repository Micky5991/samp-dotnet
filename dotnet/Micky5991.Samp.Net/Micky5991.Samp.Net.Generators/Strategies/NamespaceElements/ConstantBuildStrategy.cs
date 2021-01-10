using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using Micky5991.Samp.Net.Generators.Contracts;
using Micky5991.Samp.Net.Generators.Data;
using Micky5991.Samp.Net.Generators.Extensions;

namespace Micky5991.Samp.Net.Generators.Strategies.NamespaceElements
{
    public class ConstantBuildStrategy : IElementBuildStrategy
    {
        private readonly Regex constantExpression;

        private readonly SortedDictionary<string, (string Type, string Value)> extractedConstants = new();
        private bool enumsBuilt = false;

        public ConstantBuildStrategy()
        {
            this.constantExpression = new Regex(@"^const (?<type>.*)?\s+(?<name>[A-z0-9_]+)\s+=\s*(?<value>.*);$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        }

        public bool IsLineAccepted(string line)
        {
            return this.constantExpression.IsMatch(line);
        }

        public IdlNamespaceElement Parse(string line)
        {
            var match = this.constantExpression.Match(line);

            var name = match.Groups["name"].Value;
            var type = match.Groups["type"].Value;
            var value = match.Groups["value"].Value;

            this.extractedConstants[name] = (type, value);

            return new IdlConstant(type, name, value);
        }

        public void Build(IdlNamespaceElement element, IList<string> constantPrefixes, BuilderTargetCollection builderTargets, int indent)
        {
            if (this.enumsBuilt)
            {
                return;
            }

            this.BuildEnums(constantPrefixes, builderTargets, indent);
            this.BuildConstants(builderTargets, indent);

            this.enumsBuilt = true;
        }

        private void BuildConstants(BuilderTargetCollection builderTargets, int indent)
        {
            foreach (var extractedConstant in this.extractedConstants)
            {
                var name = extractedConstant.Key;
                var (type, value) = extractedConstant.Value;

                builderTargets[BuilderTarget.Constants]
                    .AppendLine($"public const {type} {name.ConvertToPascalCase()} = {value};".Indent(indent));
            }
        }

        private void BuildEnums(IList<string> constantPrefixes, BuilderTargetCollection builderTargets, int indent)
        {
            var builder = builderTargets[BuilderTarget.Types];

            var constants = new SortedDictionary<string, (string Type, string Value)>(this.extractedConstants);

            foreach (var constantPrefix in constantPrefixes)
            {
                builder.AppendLine($"public enum {constantPrefix.ConvertToPascalCase()}".Indent(indent - 1));
                builder.AppendLine("{".Indent(indent -1 ));

                var prefixedConstants = constants
                                        .Where(x => x.Key.StartsWith(constantPrefix))
                                        .OrderBy(x => x.Value.Value)
                                        .ToList();

                foreach (var constant in prefixedConstants)
                {
                    var name = constant.Key;
                    var (type, value) = constant.Value;

                    var subname = name.Substring(constantPrefix.Length + 1);

                    builder.AppendLine($"{subname.ConvertToPascalCase().Indent(indent)} = {value},");
                    constants.Remove(name);
                }

                builder.AppendLine("}".Indent(indent - 1));
                builder.AppendLine();
            }
        }

    }
}
