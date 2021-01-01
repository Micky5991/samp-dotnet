using System;

namespace Micky5991.Samp.Net.Generators.Data
{
    public class IdlFunctionParameter
    {
        public string Type { get; }

        public string Name { get; }

        public IdlAttribute Attribute { get; }

        public string DefaultValue { get; }

        public IdlFunctionParameter(string type, string name, IdlAttribute attribute, string defaultValue)
        {
            this.Type = type;
            this.Name = name;
            this.Attribute = attribute;
            this.DefaultValue = defaultValue;
        }
    }
}
