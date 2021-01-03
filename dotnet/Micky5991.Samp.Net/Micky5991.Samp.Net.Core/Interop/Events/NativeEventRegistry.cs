using Micky5991.Samp.Net.Core.Interfaces.Events;

namespace Micky5991.Samp.Net.Core.Interop.Events
{
    public class NativeEventRegistry : INativeEventRegistry
    {
        public void RegisterEvent(string name, string format)
        {
            Native.RegisterEvent(name, format);
        }
    }
}
