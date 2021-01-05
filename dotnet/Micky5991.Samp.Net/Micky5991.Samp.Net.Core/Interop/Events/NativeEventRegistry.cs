using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using JetBrains.Annotations;
using Micky5991.EventAggregator.Interfaces;
using Micky5991.Samp.Net.Core.Interfaces.Events;

namespace Micky5991.Samp.Net.Core.Interop.Events
{
    public class NativeEventRegistry : INativeEventRegistry
    {
        private readonly IEventAggregator eventAggregator;

        private readonly IDictionary<string, INativeEventRegistry.BuildEventDelegate> builders;

        private GCHandle nativeEventInvoker;

        public NativeEventRegistry(IEventAggregator eventAggregator)
        {
            this.eventAggregator = eventAggregator;

            this.builders = new Dictionary<string, INativeEventRegistry.BuildEventDelegate>();
        }

        public virtual void RegisterEvent(string name, string format, INativeEventRegistry.BuildEventDelegate builder)
        {
            this.builders[name] = builder;

            Native.RegisterEvent(name, format);
        }

        public void RegisterEvents(INativeEventCollection collection)
        {
            foreach (var definition in collection.Values)
            {
                this.RegisterEvent(definition.Name, definition.Format, definition.Builder);
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
            INativeEventRegistry.EventInvokerDelegate invoker = this.InvokeEvent;

            this.nativeEventInvoker = GCHandle.Alloc(invoker);

            Native.AttachEventHandler(invoker);
        }

        public virtual void InvokeEvent(string name, CallbackArgument[]? arguments, int argumentAmount)
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

        public void Dispose()
        {
            this.nativeEventInvoker.Free();

            GC.SuppressFinalize(this);
        }
    }
}
