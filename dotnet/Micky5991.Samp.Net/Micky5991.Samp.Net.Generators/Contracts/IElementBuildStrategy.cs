using System.Collections.Generic;
using Micky5991.Samp.Net.Generators.Data;

namespace Micky5991.Samp.Net.Generators.Contracts
{
    public interface IElementBuildStrategy
    {
        bool IsLineAccepted(string line);

        IdlNamespaceElement Parse(string line);

        void Build(IdlNamespaceElement element, IList<string> constantPrefixes, BuilderTargetCollection builderTargets, int indent);
    }
}
