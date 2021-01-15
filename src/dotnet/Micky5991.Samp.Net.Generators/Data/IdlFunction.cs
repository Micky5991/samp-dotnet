using System.Collections.Generic;

namespace Micky5991.Samp.Net.Generators.Data
{
    public class IdlFunction : IdlNamespaceElement
    {
        public string Name { get; }

        public string ReturnType { get; }

        public IdlAttribute Attribute { get; }

        public IList<IdlFunctionParameter> Parameters { get; }

        public IdlFunction(string name, string returnType, IdlAttribute attribute, IList<IdlFunctionParameter> parameters)
        {
            this.Name = name;
            this.ReturnType = returnType;
            this.Attribute = attribute;
            this.Parameters = parameters;
        }
    }
}
