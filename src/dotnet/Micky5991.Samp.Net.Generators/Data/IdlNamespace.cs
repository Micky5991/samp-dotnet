using System.Collections.Generic;
using Micky5991.Samp.Net.Generators.Contracts;

namespace Micky5991.Samp.Net.Generators.Data
{
    public class IdlNamespace
    {
        public string Name { get; }

        public string Fullname { get; }

        public IList<(IElementBuildStrategy Strategy, IdlNamespaceElement Element)> Elements { get; }

        public IList<string> ConstantPrefixes { get; }

        public IdlNamespace(string name, string fullname, IList<(IElementBuildStrategy Strategy, IdlNamespaceElement Element)> elements, IList<string> constantPrefixes)
        {
            this.Name = name;
            this.Fullname = fullname;
            this.Elements = elements;
            this.ConstantPrefixes = constantPrefixes;
        }

    }
}
