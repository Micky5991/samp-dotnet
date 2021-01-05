using Micky5991.Samp.Net.Core.Interfaces.Events;

namespace Micky5991.Samp.Net.Core.Interop.Data
{
    public readonly struct NativeEventDefinition
    {
        public string Name { get; }

        public string Format { get; }

        public INativeEventRegistry.BuildEventDelegate Builder { get; }

        public NativeEventDefinition(string name, string format, INativeEventRegistry.BuildEventDelegate builder)
        {
            this.Name = name;
            this.Format = format;
            this.Builder = builder;
        }
    }
}
