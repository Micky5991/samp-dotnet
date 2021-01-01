using System.Collections.Generic;
using Micky5991.Samp.Net.Generators.Contracts;

namespace Micky5991.Samp.Net.Generators.Data
{
    public class IdlNamespace
    {
        public string Name { get; }

        public IList<(IElementBuildStrategy Strategy, IdlNamespaceElement Element)> Elements { get; }

        public IdlNamespace(string name, IList<(IElementBuildStrategy Strategy, IdlNamespaceElement Element)> elements)
        {
            this.Name = name;
            this.Elements = elements;
        }

    }
}
