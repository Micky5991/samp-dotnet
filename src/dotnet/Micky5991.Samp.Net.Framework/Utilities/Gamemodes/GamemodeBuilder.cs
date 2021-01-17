using Micky5991.EventAggregator.Interfaces;
using Micky5991.EventAggregator.Services;
using Micky5991.Samp.Net.Core.Interfaces.Events;
using Micky5991.Samp.Net.Core.Interfaces.Logging;
using Micky5991.Samp.Net.Core.Interop;
using Micky5991.Samp.Net.Core.Interop.Events;
using Micky5991.Samp.Net.Core.Natives.Actor;
using Micky5991.Samp.Net.Core.Natives.Objects;
using Micky5991.Samp.Net.Core.Natives.Players;
using Micky5991.Samp.Net.Core.Natives.Samp;
using Micky5991.Samp.Net.Core.Natives.Vehicles;
using Micky5991.Samp.Net.Core.Threading;
using Micky5991.Samp.Net.Framework.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Micky5991.Samp.Net.Framework.Utilities.Gamemodes
{
    /// <summary>
    /// Type that registers typical core services to the service container.
    /// </summary>
    public class GamemodeBuilder
    {
        /// <summary>
        /// Registers all core services provided by SAMP.Net.
        /// </summary>
        /// <param name="serviceCollection">Service collection to register all services to.</param>
        public virtual void AddServices(IServiceCollection serviceCollection)
        {
            this.AddGamemodeStarter(serviceCollection);
            this.AddLoggerHandler(serviceCollection);
            this.AddEventAggregator(serviceCollection);
            this.AddSynchronizationServices(serviceCollection);
            this.AddNativeEventHandling(serviceCollection);
            this.AddNativeEvents(serviceCollection);
            this.AddNatives(serviceCollection);
        }

        /// <summary>
        /// Should register the gamemode starter that should boot up the gamemode. Override to replace the default
        /// implementation with your custom starter.
        /// </summary>
        /// <param name="serviceCollection">Service collection to add the starter to.</param>
        protected virtual void AddGamemodeStarter(IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<IGamemodeStarter, GamemodeStarter>();
        }

        /// <summary>
        /// Should register the native samp logger that listens to all samp logs. Override to replace the default
        /// implementation with your custom listener.
        /// </summary>
        /// <param name="serviceCollection">Service collection to add the logger to.</param>
        protected virtual void AddLoggerHandler(IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<ISampLoggerHandler, SampLoggerHandler>();
        }

        /// <summary>
        /// Should register the used event aggregator. It still needs to implement the interface <see cref="IEventAggregator"/>.
        /// Override to replace the default implementation with your custom event aggregator.
        /// </summary>
        /// <param name="serviceCollection">Service collection to add the event aggregator to.</param>
        protected virtual void AddEventAggregator(IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<IEventAggregator, EventAggregatorService>();
        }

        /// <summary>
        /// Should register the used event aggregator. It still needs to implement the interface <see cref="IEventAggregator"/>.
        /// Override to replace the default implementation with your custom event aggregator.
        /// </summary>
        /// <param name="serviceCollection">Service collection to add the event aggregator to.</param>
        protected virtual void AddSynchronizationServices(IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<SampSynchronizationContext>();
        }

        /// <summary>
        /// Add services for native event handling. Override to replace the default implementation with your custom
        /// event handling.
        /// </summary>
        /// <param name="serviceCollection">Service collection to add the event handlers to.</param>
        protected virtual void AddNativeEventHandling(IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<NativeTypeConverter>()
                             .AddSingleton<INativeEventRegistry, NativeEventRegistry>();
        }

        /// <summary>
        /// Registers all default event collections to the service collection. Override to replace the default
        /// implementation with your custom native event collections.
        /// </summary>
        /// <param name="serviceCollection">Service collection to add the event collections to.</param>
        protected virtual void AddNativeEvents(IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<INativeEventCollectionFactory, SampEventCollectionFactory>()
                             .AddTransient<INativeEventCollectionFactory, VehiclesEventCollectionFactory>()
                             .AddTransient<INativeEventCollectionFactory, PlayersEventCollectionFactory>()
                             .AddTransient<INativeEventCollectionFactory, ActorEventCollectionFactory>()
                             .AddTransient<INativeEventCollectionFactory, ObjectsEventCollectionFactory>();
        }

        /// <summary>
        /// Registers all available natives to the service collection seperated by namespace. Override to replace the default
        /// implementation with your custom natives.
        /// </summary>
        /// <param name="serviceCollection">Service collection to add the natives to.</param>
        protected virtual void AddNatives(IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<ISampNatives, SampNatives>()
                             .AddTransient<IVehiclesNatives, VehiclesNatives>()
                             .AddTransient<IPlayersNatives, PlayersNatives>()
                             .AddTransient<IActorNatives, ActorNatives>()
                             .AddTransient<IObjectsNatives, ObjectsNatives>();
        }
    }
}
