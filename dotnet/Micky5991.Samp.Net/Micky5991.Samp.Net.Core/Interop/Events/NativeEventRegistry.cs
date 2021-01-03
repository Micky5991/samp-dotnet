using System;
using Micky5991.Samp.Net.Core.Interfaces.Events;

namespace Micky5991.Samp.Net.Core.Interop.Events
{
    public class NativeEventRegistry : INativeEventRegistry
    {
        public void RegisterEvent(string name, string format)
        {
            Native.RegisterEvent(name, format);
        }

        public void InvokeEvent(string name, CallbackArgument[]? arguments, int argumentAmount)
        {
            if (arguments == null)
            {
                arguments = new CallbackArgument[0];
            }

            Console.WriteLine("C#: Invoke event " + name + " -> " + arguments.Length);
        }
    }
}
