using System;
using System.Runtime.InteropServices;
using Micky5991.Samp.Net.Core.Interop.Events;
using Micky5991.Samp.Net.Core.NativeEvents;

namespace Micky5991.Samp.Net.Core.Interfaces.Events
{
    public interface INativeEventRegistry
    {
        public delegate INativeEvent BuildEventDelegate(object[] arguments);

        public void RegisterEvent(string name, string format, BuildEventDelegate builder);

        public void InvokeEvent(string name, CallbackArgument[]? arguments, int argumentAmount);

    }
}
