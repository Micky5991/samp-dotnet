using Micky5991.Samp.Net.Core.Interop.Events;

namespace Micky5991.Samp.Net.Core.Interfaces.Events
{
    public interface INativeEventRegistry
    {

        public void RegisterEvent(string name, string format);

        public void InvokeEvent(string name, CallbackArgument[]? arguments, int argumentAmount);

    }
}
