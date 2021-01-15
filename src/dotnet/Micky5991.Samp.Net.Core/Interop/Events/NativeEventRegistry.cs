using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Micky5991.EventAggregator.Interfaces;
using Micky5991.Samp.Net.Core.Interfaces.Events;
using Micky5991.Samp.Net.Core.Interop.Data;

namespace Micky5991.Samp.Net.Core.Interop.Events
{
    public class NativeEventRegistry : INativeEventRegistry
    {
        private readonly IEventAggregator eventAggregator;

        private readonly IDictionary<string, (INativeEventRegistry.BuildEventDelegate Builder, bool BadReturnValue)> builders;

        private GCHandle nativeEventInvoker;

        public NativeEventRegistry(IEventAggregator eventAggregator)
        {
            this.eventAggregator = eventAggregator;

            this.builders = new Dictionary<string, (INativeEventRegistry.BuildEventDelegate Builder, bool defaultReturnValue)>();
        }

        public virtual void RegisterEvent(string name, string format, INativeEventRegistry.BuildEventDelegate builder, bool defaultReturnValue)
        {
            this.builders[name] = (builder, defaultReturnValue);

            Native.RegisterEvent(name, format);
        }

        public void RegisterEvents(INativeEventCollection collection)
        {
            foreach (var definition in collection.Values)
            {
                this.RegisterEvent(definition.Name, definition.Format, definition.Builder, definition.BadReturnValue);
            }
        }

        public void RegisterEvents(IEnumerable<INativeEventCollection> collections)
        {
            foreach (var collection in collections)
            {
                this.RegisterEvents(collection);
            }
        }

        public void AttachEventInvoker()
        {
            Native.EventInvokerDelegate invoker = this.InvokeEvent;

            this.nativeEventInvoker = GCHandle.Alloc(invoker);

            Native.AttachEventHandler(invoker);
        }

        public virtual EventInvokeResult InvokeEvent(string name, CallbackArgument[]? arguments, int argumentAmount)
        {
            try
            {
                if (arguments == null)
                {
                    arguments = new CallbackArgument[0];
                }

                if (this.builders.TryGetValue(name, out var definition) == false)
                {
                    Console.WriteLine($"Unable to find builder for event {name}.");

                    return new EventInvokeResult(true, false);
                }

                var convertedArguments = arguments.Select(x => x.GetValue()).ToArray();
                var (builder, badReturnValue) = definition;

                var eventInstance = builder(convertedArguments);

                this.eventAggregator.Publish(eventInstance);
                if (eventInstance is ICancellableEvent { Cancelled: true })
                {
                    return new EventInvokeResult(false, badReturnValue);
                }

                return new EventInvokeResult(true, badReturnValue == false);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error: {e.Message}");
                Console.WriteLine(e.StackTrace);
            }

            return new EventInvokeResult(true, false);
        }

        public void Dispose()
        {
            this.nativeEventInvoker.Free();

            GC.SuppressFinalize(this);
        }
    }
}
