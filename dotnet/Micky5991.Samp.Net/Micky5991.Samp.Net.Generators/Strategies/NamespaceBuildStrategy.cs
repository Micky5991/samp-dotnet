using System.Collections.Generic;
using System.IO;
using System.Linq;
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

            var nameMatch = Regex.Match(filename, @"natives\.(?<full>(?:a_)?(?<core>[A-z0-9_-]+)).idl");

            var constantsFileName = $"{Path.GetDirectoryName(filename)}{Path.DirectorySeparatorChar}constants.{nameMatch.Groups["full"]}.txt";
            var constantPrefixes = File.Exists(constantsFileName) ? this.ParseConstantPrefixes(new StreamReader(constantsFileName)) : new List<string>();

            return new IdlNamespace(nameMatch.Groups["core"].Value, nameMatch.Groups["full"].Value, elements, constantPrefixes);
        }

        private IList<string> ParseConstantPrefixes(TextReader stream)
        {
            List<string> prefixes = new();

            string line;
            while ((line = stream.ReadLine()) != null)
            {
                if (line.Length <= 0)
                {
                    continue;
                }

                prefixes.Add(line.Trim());
            }

            prefixes = prefixes.OrderByDescending(x => x.Split('_').Length).ToList();

            return prefixes;
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

        public void Build(BuilderTargetCollection builderTargets, IdlNamespace idlNamespace, int indent)
        {
            var namespaceBuilderTargets = new BuilderTargetCollection
            {
                BuilderTarget.InterfaceSignatures,
                BuilderTarget.Delegates,
                BuilderTarget.Functions,
                BuilderTarget.Events,
                BuilderTarget.Types,
                BuilderTarget.Constants
            };

            namespaceBuilderTargets.Parent = builderTargets;

            foreach (var (strategy, element) in idlNamespace.Elements)
            {
                strategy.Build(element, idlNamespace.ConstantPrefixes, namespaceBuilderTargets, indent + 2);
            }

            this.BuildNamespaceConstants(namespaceBuilderTargets, idlNamespace, indent + 1);
            this.BuildNamespaceEventsClass(namespaceBuilderTargets, idlNamespace, indent + 1);
            this.BuildNamespaceNativesInterface(namespaceBuilderTargets, idlNamespace, indent + 1);
            this.BuildNamespaceNativesClass(namespaceBuilderTargets, idlNamespace, indent + 1);

            var namespaceTarget = builderTargets[BuilderTarget.Namespaces];

            namespaceTarget.AppendLine($"namespace Micky5991.Samp.Net.Core.Natives.{idlNamespace.Name.ConvertToPascalCase()}".Indent(indent));
            namespaceTarget.AppendLine("{".Indent(indent));

            namespaceTarget.AppendLine(namespaceBuilderTargets[BuilderTarget.Types].ToString());

            namespaceTarget.AppendLine("}".Indent(indent));
        }

        private void BuildNamespaceConstants(BuilderTargetCollection buildTargets, IdlNamespace idlNamespace, int indent)
        {
            var stringBuilder = buildTargets[BuilderTarget.Types];

            stringBuilder.AppendLine($"public static class {idlNamespace.Name.ConvertToPascalCase()}Constants".Indent(indent));
            stringBuilder.AppendLine("{".Indent(indent));

            stringBuilder.AppendLine(buildTargets[BuilderTarget.Constants].ToString());

            stringBuilder.AppendLine("}".Indent(indent));
        }

        private void BuildNamespaceNativesInterface(BuilderTargetCollection buildTargets, IdlNamespace idlNamespace, int indent)
        {
            var stringBuilder = buildTargets[BuilderTarget.Types];

            stringBuilder.AppendLine($"public interface I{idlNamespace.Name.ConvertToPascalCase()}Natives : INatives".Indent(indent));
            stringBuilder.AppendLine("{".Indent(indent));

            stringBuilder.Append(buildTargets[BuilderTarget.InterfaceSignatures].ToString());

            stringBuilder.AppendLine("}".Indent(indent));
            stringBuilder.AppendLine();
        }

        private void BuildNamespaceNativesClass(BuilderTargetCollection buildTargets, IdlNamespace idlNamespace, int indent)
        {
            var stringBuilder = buildTargets[BuilderTarget.Types];

            stringBuilder.AppendLine($"public class {idlNamespace.Name.ConvertToPascalCase()}Natives : I{idlNamespace.Name.ConvertToPascalCase()}Natives".Indent(indent));
            stringBuilder.AppendLine("{".Indent(indent));

            // Field
            stringBuilder.AppendLine("private NativeTypeConverter typeConverter;".Indent(indent + 1));
            stringBuilder.AppendLine();

            stringBuilder.Append(buildTargets[BuilderTarget.Delegates].ToString());

            // Constructor
            stringBuilder.AppendLine($@"public {idlNamespace.Name.ConvertToPascalCase()}Natives(NativeTypeConverter typeConverter)".Indent(indent + 1));
            stringBuilder.AppendLine("{".Indent(indent + 1));

            stringBuilder.AppendLine("this.typeConverter = typeConverter;".Indent(indent + 2));

            stringBuilder.AppendLine("}".Indent(indent + 1));
            stringBuilder.AppendLine();

            // Body
            stringBuilder.Append(buildTargets[BuilderTarget.Functions].ToString());

            stringBuilder.AppendLine("}".Indent(indent));
            stringBuilder.AppendLine();
        }

        private void BuildNamespaceEventsClass(BuilderTargetCollection buildTargets, IdlNamespace idlNamespace, int indent)
        {
            var stringBuilder = buildTargets[BuilderTarget.Types];

            stringBuilder.AppendLine($"public class {idlNamespace.Name.ConvertToPascalCase()}EventCollectionFactory : Micky5991.Samp.Net.Core.Interfaces.Events.INativeEventCollectionFactory".Indent(indent));
            stringBuilder.AppendLine("{".Indent(indent));

            // Constructor
            stringBuilder.AppendLine($@"public Micky5991.Samp.Net.Core.Interfaces.Events.INativeEventCollection Build()".Indent(indent + 1));
            stringBuilder.AppendLine("{".Indent(indent + 1));

            stringBuilder.AppendLine("return new Micky5991.Samp.Net.Core.Interop.Events.NativeEventCollection {".Indent(indent + 2));

            stringBuilder.Append(buildTargets[BuilderTarget.Events].ToString());

            stringBuilder.AppendLine("};".Indent(indent + 2));
            stringBuilder.AppendLine("}".Indent(indent + 1));

            stringBuilder.AppendLine("}".Indent(indent));
            stringBuilder.AppendLine();
        }
    }
}
