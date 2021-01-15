using System.Diagnostics;
using System.Text;
using System.Linq;
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

            var code = builder.GenerateCode(additionalFiles);

            context.AddSource("SampNatives.cs", SourceText.From(code, Encoding.UTF8));
        }
    }
}
