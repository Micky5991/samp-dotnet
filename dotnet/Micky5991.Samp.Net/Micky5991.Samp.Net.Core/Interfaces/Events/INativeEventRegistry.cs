using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using JetBrains.Annotations;
using Micky5991.Samp.Net.Core.Interop.Events;
using Micky5991.Samp.Net.Core.NativeEvents;

namespace Micky5991.Samp.Net.Core.Interfaces.Events
{
    [PublicAPI]
    public interface INativeEventRegistry : IDisposable
    {
        public delegate void EventInvokerDelegate(string eventName, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 2)] CallbackArgument[]? argments, int argumentAmount);

        public delegate INativeEvent BuildEventDelegate(object[] arguments);

        public void RegisterEvent(string name, string format, BuildEventDelegate builder);

        public void RegisterEvents(INativeEventCollection collection);
        public void RegisterEvents(IEnumerable<INativeEventCollection> collections);

        public void AttachEventInvoker();

        public void InvokeEvent(string name, CallbackArgument[]? arguments, int argumentAmount);

    }
}
