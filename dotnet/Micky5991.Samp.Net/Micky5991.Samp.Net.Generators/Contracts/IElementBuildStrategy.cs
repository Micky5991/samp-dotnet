using System.Text;
using Micky5991.Samp.Net.Generators.Data;

namespace Micky5991.Samp.Net.Generators.Contracts
{
    public interface IElementBuildStrategy
    {
        bool IsLineAccepted(string line);

        IdlNamespaceElement Parse(string line);

        void Build(
            IdlNamespaceElement element,
            StringBuilder typesBuilder,
            StringBuilder delegateBuilder,
            StringBuilder interfaceSignaturesBuilder,
            StringBuilder functionBuilder,
            int indent);
    }
}
