namespace Micky5991.Samp.Net.Generators.Data
{
    public class IdlConstant : IdlNamespaceElement
    {
        public string Type { get; }

        public string Name { get; }

        public string Value { get; }

        public IdlConstant(string type, string name, string value)
        {
            this.Type = type;
            this.Name = name;
            this.Value = value;
        }
    }
}
