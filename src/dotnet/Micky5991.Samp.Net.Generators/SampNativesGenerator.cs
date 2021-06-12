using System.Diagnostics;
using System.Text;
using System.Linq;
using Micky5991.Samp.Net.Generators.Extensions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace Micky5991.Samp.Net.Generators
{
    [Generator]
    public class SampNativeGenerator : ISourceGenerator
    {
        public void Initialize(GeneratorInitializationContext context)
        {
// #if DEBUG
//             if (Debugger.IsAttached == false)
//             {
//                 Debugger.Launch();
//             }
// #endif
        }

        public void Execute(GeneratorExecutionContext context)
        {
            var builder = new SampNativeBuilder();

            var additionalFiles = context.AdditionalFiles
                                         .Where(x => builder.DoesFilenameMatch(x.Path))
                                         .Select(x => x.Path)
                                         .ToList();

            foreach (var additionalFile in additionalFiles)
            {
                var code = builder.GenerateCode(additionalFile, out var idlNamespace);

                context.AddSource($"{idlNamespace.Name.ConvertToPascalCase()}.cs", SourceText.From(code, Encoding.UTF8));
            }

        }
    }
}
