using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using Micky5991.Samp.Net.Generators.Contracts;
using Micky5991.Samp.Net.Generators.Extensions;
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

        public string GenerateCode(IList<string> filePaths)
        {
            var parameterBuildStrategy = new ParameterBuildStrategy();
            var elementBuildStrategies = new List<IElementBuildStrategy>
            {
                new NativeBuildStrategy(parameterBuildStrategy),
                new CallbackBuildStrategy(parameterBuildStrategy),
                new ConstantBuildStrategy(),
            };
            var namespaceBuildStrategy = new NamespaceBuildStrategy(elementBuildStrategies);

            var namespacesStringBuilder = new StringBuilder();
            foreach (var path in filePaths)
            {
                using var stream = new StreamReader(path);

                var idlNamespace = namespaceBuildStrategy.Parse(path, stream);

                namespaceBuildStrategy.Build(namespacesStringBuilder, idlNamespace, 1);
            }

            var sourceBuilder = new StringBuilder();
            sourceBuilder.Append(@"using System;
using Micky5991.Samp.Net.Core.Interop;

namespace Micky5991.Samp.Net.Core.Natives
{ 
");

            // ReSharper disable once RedundantToStringCall
            sourceBuilder.Append(namespacesStringBuilder.ToString());

            sourceBuilder.AppendLine("}");

            return sourceBuilder.ToString();
        }
    }
}
