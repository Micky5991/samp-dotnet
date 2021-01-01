using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using Micky5991.Samp.Net.Generators.Contracts;
using Micky5991.Samp.Net.Generators.Data;
using Micky5991.Samp.Net.Generators.Extensions;

namespace Micky5991.Samp.Net.Generators.Strategies
{
    public class NamespaceBuildStrategy
    {
        private readonly IList<IElementBuildStrategy> elementBuildStrategies;

        public NamespaceBuildStrategy(IList<IElementBuildStrategy> elementBuildStrategies)
        {
            this.elementBuildStrategies = elementBuildStrategies;
        }

        public IdlNamespace Parse(string filename, TextReader stream)
        {
            var elements = this.ParseElements(stream);
            var nameMatch = Regex.Match(filename, @"natives\.(?:a_)?([A-z0-9_-]+).idl");

            return new IdlNamespace(nameMatch.Groups[1].Value, elements);
        }

        private IList<(IElementBuildStrategy Strategy, IdlNamespaceElement Element)> ParseElements(TextReader stream)
        {
            var elements = new List<(IElementBuildStrategy Strategy, IdlNamespaceElement Element)>();

            string line;
            while ((line = stream.ReadLine()) != null)
            {
                if (line.Length <= 0)
                {
                    continue;
                }

                foreach (var elementBuildStrategy in this.elementBuildStrategies)
                {
                    if (elementBuildStrategy.IsLineAccepted(line))
                    {
                        elements.Add((elementBuildStrategy, elementBuildStrategy.Parse(line)));

                        break;
                    }
                }
            }

            return elements;
        }

        public void Build(StringBuilder stringBuilder, IdlNamespace idlNamespace, int indent)
        {
            var typesBuilder = new StringBuilder();
            var delegateBuilder = new StringBuilder();
            var functionBuilder = new StringBuilder();

            foreach (var (strategy, element) in idlNamespace.Elements)
            {
                strategy.Build(element, typesBuilder, delegateBuilder, functionBuilder, indent + 1);
            }

            stringBuilder.AppendLine($"public static class {idlNamespace.Name.ConvertToPascalCase()}".Indent(indent));
            stringBuilder.AppendLine("{".Indent(indent));

            stringBuilder.Append(typesBuilder.ToString());
            stringBuilder.Append(delegateBuilder.ToString());
            stringBuilder.Append(functionBuilder.ToString());

            stringBuilder.AppendLine("}".Indent(indent));
            stringBuilder.AppendLine();
        }
    }
}
