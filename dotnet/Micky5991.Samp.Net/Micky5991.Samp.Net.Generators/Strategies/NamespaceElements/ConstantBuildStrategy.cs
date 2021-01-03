using System.Text;
using System.Text.RegularExpressions;
using Micky5991.Samp.Net.Generators.Contracts;
using Micky5991.Samp.Net.Generators.Data;

namespace Micky5991.Samp.Net.Generators.Strategies.NamespaceElements
{
    public class ConstantBuildStrategy : IElementBuildStrategy
    {
        private readonly Regex constantExpression;

        public ConstantBuildStrategy()
        {
            this.constantExpression = new Regex(@"^const (?<type>.*)?\s+(?<name>[A-z0-9_]+)\s+=\s*(?<value>.*);$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        }

        public bool IsLineAccepted(string line)
        {
            return false;
        }

        public IdlNamespaceElement Parse(string line)
        {
            throw new System.NotImplementedException();
        }

        public void Build(IdlNamespaceElement element, BuilderTargetCollection builderTargets, int indent)
        {
            throw new System.NotImplementedException();
        }
    }
}
