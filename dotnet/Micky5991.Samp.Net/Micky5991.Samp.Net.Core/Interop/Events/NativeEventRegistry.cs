using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Micky5991.EventAggregator.Interfaces;
using Micky5991.Samp.Net.Core.Interfaces.Events;

namespace Micky5991.Samp.Net.Core.Interop.Events
{
    public class NativeEventRegistry : INativeEventRegistry
    {
        private readonly IEventAggregator eventAggregator;

        private readonly IDictionary<string, INativeEventRegistry.BuildEventDelegate> builders =
            new Dictionary<string, INativeEventRegistry.BuildEventDelegate>();

        public NativeEventRegistry(IEventAggregator eventAggregator)
        {
            this.eventAggregator = eventAggregator;
        }

        public void RegisterEvent(string name, string format, INativeEventRegistry.BuildEventDelegate builder)
        {
            this.builders[name] = builder;

            Native.RegisterEvent(name, format);
        }

        public void InvokeEvent(string name, CallbackArgument[]? arguments, int argumentAmount)
        {
            try
            {
                if (arguments == null)
                {
                    arguments = new CallbackArgument[0];
                }

                if (this.builders.TryGetValue(name, out var builder) == false)
                {
                    Console.WriteLine($"Unable to find builder for event {name}.");

                    return;
                }

                var convertedArguments = arguments.Select(x => x.GetValue()).ToArray();

                var eventInstance = builder(convertedArguments);

                this.eventAggregator.PublishSync(eventInstance);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error: {e.Message}");
                Console.WriteLine(e.StackTrace);
            }
        }
    }
}
