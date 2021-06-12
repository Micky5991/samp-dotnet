using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using Micky5991.Samp.Net.Generators.Contracts;
using Micky5991.Samp.Net.Generators.Data;
using Micky5991.Samp.Net.Generators.Strategies;
using Micky5991.Samp.Net.Generators.Strategies.NamespaceElements;
using Micky5991.Samp.Net.Generators.Strategies.Parameters;

namespace Micky5991.Samp.Net.Generators
{
    public class SampNativeBuilder
    {
        private readonly Regex fileNameExpression;

        public SampNativeBuilder()
        {
            this.fileNameExpression = new Regex(@"natives\.(?:a_)?([A-z0-9_-]+).idl", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        }

        public bool DoesFilenameMatch(string filename)
        {
            return this.fileNameExpression.IsMatch(filename);
        }

        public string GenerateCode(string filePath, out IdlNamespace idlNamespace)
        {
            var builderTargets = new BuilderTargetCollection
            {
                BuilderTarget.Namespaces,
            };

            var parameterBuildStrategy = new ParameterBuildStrategy();
            var elementBuildStrategies = new List<IElementBuildStrategy>
            {
                new NativeBuildStrategy(parameterBuildStrategy),
                new CallbackBuildStrategy(parameterBuildStrategy),
                new ConstantBuildStrategy(),
            };
            var namespaceBuildStrategy = new NamespaceBuildStrategy(elementBuildStrategies);

            using var stream = new StreamReader(filePath);

            idlNamespace = namespaceBuildStrategy.Parse(filePath, stream);

            builderTargets[BuilderTarget.Namespaces].AppendLine($"// {filePath}");
            namespaceBuildStrategy.Build(builderTargets, idlNamespace, 0);

            var sourceBuilder = new StringBuilder();
            sourceBuilder.Append(@"using System;
using Microsoft.Extensions.Logging;
using Micky5991.Samp.Net.Core.Interop;
using Micky5991.Samp.Net.Core.Interfaces.Natives;
using Micky5991.Samp.Net.Core.Interfaces.Interop;

");

            // ReSharper disable once RedundantToStringCall
            sourceBuilder.Append(builderTargets[BuilderTarget.Namespaces].ToString());

            return sourceBuilder.ToString();
        }
    }
}
