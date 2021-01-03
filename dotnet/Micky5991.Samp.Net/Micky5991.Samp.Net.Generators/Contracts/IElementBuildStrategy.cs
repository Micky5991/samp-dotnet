using System.Text;
using Micky5991.Samp.Net.Generators.Data;

namespace Micky5991.Samp.Net.Generators.Contracts
{
    public interface IElementBuildStrategy
    {
        bool IsLineAccepted(string line);

        IdlNamespaceElement Parse(string line);

        void Build(IdlNamespaceElement element, BuilderTargetCollection builderTargets, int indent);
    }
}
