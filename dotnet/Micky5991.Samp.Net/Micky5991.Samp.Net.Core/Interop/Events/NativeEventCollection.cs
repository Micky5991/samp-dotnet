using System.Collections.Generic;
using Micky5991.Samp.Net.Core.Interfaces.Events;
using Micky5991.Samp.Net.Core.Interop.Data;

namespace Micky5991.Samp.Net.Core.Interop.Events
{
    public class NativeEventCollection : Dictionary<string, NativeEventDefinition>, INativeEventCollection
    {
        public void Add(string name, string format, INativeEventRegistry.BuildEventDelegate builderDelegate)
        {
            this.Add(name, new NativeEventDefinition(name, format, builderDelegate));
        }
    }
}
