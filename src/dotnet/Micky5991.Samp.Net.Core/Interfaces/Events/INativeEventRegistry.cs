using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Micky5991.Samp.Net.Core.Interop.Data;
using Micky5991.Samp.Net.Core.Interop.Events;

namespace Micky5991.Samp.Net.Core.Interfaces.Events
{
    [PublicAPI]
    public interface INativeEventRegistry : IDisposable
    {
        public delegate INativeEvent BuildEventDelegate(object[] arguments);

        public void RegisterEvent(string name, string format, BuildEventDelegate builder, bool defaultReturnValue);

        public void RegisterEvents(INativeEventCollection collection);
        public void RegisterEvents(IEnumerable<INativeEventCollection> collections);

        public void AttachEventInvoker();

        public EventInvokeResult InvokeEvent(string name, CallbackArgument[]? arguments, int argumentAmount);

    }
}
