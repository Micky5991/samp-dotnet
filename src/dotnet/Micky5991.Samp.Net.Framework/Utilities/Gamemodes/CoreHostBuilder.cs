using System;
using Dawn;
using Micky5991.EventAggregator.Interfaces;
using Micky5991.EventAggregator.Services;
using Micky5991.Samp.Net.Core.Interfaces.Events;
using Micky5991.Samp.Net.Core.Interfaces.Interop;
using Micky5991.Samp.Net.Core.Interfaces.Logging;
using Micky5991.Samp.Net.Core.Interop;
using Micky5991.Samp.Net.Core.Interop.Events;
using Micky5991.Samp.Net.Core.Natives.Actor;
using Micky5991.Samp.Net.Core.Natives.Objects;
using Micky5991.Samp.Net.Core.Natives.Players;
using Micky5991.Samp.Net.Core.Natives.Samp;
using Micky5991.Samp.Net.Core.Natives.Vehicles;
using Micky5991.Samp.Net.Core.Threading;
using Micky5991.Samp.Net.Framework.Elements.Entities.Factories;
using Micky5991.Samp.Net.Framework.Elements.Entities.Listeners;
using Micky5991.Samp.Net.Framework.Elements.Entities.Pools;
using Micky5991.Samp.Net.Framework.Interfaces;
using Micky5991.Samp.Net.Framework.Interfaces.Entities.Factories;
using Micky5991.Samp.Net.Framework.Interfaces.Entities.Pools;
using Micky5991.Samp.Net.Framework.Interfaces.Facades;
using Micky5991.Samp.Net.Framework.Interfaces.Services;
using Micky5991.Samp.Net.Framework.Interfaces.Startup;
using Micky5991.Samp.Net.Framework.Services;
using Micky5991.Samp.Net.Framework.Services.Facades;
using Micky5991.Samp.Net.Framework.Services.Syncer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;

namespace Micky5991.Samp.Net.Framework.Utilities.Gamemodes
{
    /// <summary>
    /// Type that registers typical core services to the service container.
    /// </summary>
    public class CoreHostBuilder : IExtendableHostBuilder
    {
        /// <inheritdoc />
        public virtual void ConfigureHost(IHostBuilder hostBuilder)
        {
            Guard.Argument(hostBuilder, nameof(hostBuilder)).NotNull();

            void ConfigureServices(HostBuilderContext context, IServiceCollection serviceCollection)
            {
                this.AddCoreServices(serviceCollection);
                this.AddLoggerHandler(serviceCollection);
                this.AddEventAggregator(serviceCollection);
                this.AddSynchronizationServices(serviceCollection);
                this.AddNativeEventHandling(serviceCollection);
                this.AddNativeEvents(serviceCollection);
                this.AddNatives(serviceCollection);
                this.AddEntityFactories(serviceCollection);
                this.AddEntityPools(serviceCollection);
                this.AddEntityListeners(serviceCollection);
                this.AddDialogHandler(serviceCollection);
                this.AddUtilityServices(serviceCollection);
                this.AddAuthorizationServices(serviceCollection);
            }

            hostBuilder.ConfigureServices(ConfigureServices);
        }

        /// <summary>
        /// Should register the core services needed for basic operation of the framework.
        /// </summary>
        /// <param name="serviceCollection">Target to add the services to.</param>
        protected virtual void AddCoreServices(IServiceCollection serviceCollection)
        {
            serviceCollection.AddHostedService<CoreGamemodeHostedService>();
            serviceCollection.AddSingleton<ISampThreadEnforcer, SampThreadEnforcer>();
        }

        /// <summary>
        /// Should register the native samp logger that listens to all samp logs. Override to replace the default
        /// implementation with your custom listener.
        /// </summary>
        /// <param name="serviceCollection">Target to add the services to.</param>
        protected virtual void AddLoggerHandler(IServiceCollection serviceCollection)
        {
            serviceCollection.TryAddSingleton<ISampLoggerHandler, SampLoggerHandler>();
        }

        /// <summary>
        /// Should register the used event aggregator. It still needs to implement the interface <see cref="IEventAggregator"/>.
        /// Override to replace the default implementation with your custom event aggregator.
        /// </summary>
        /// <param name="serviceCollection">Target to add the services to.</param>
        protected virtual void AddEventAggregator(IServiceCollection serviceCollection)
        {
            serviceCollection.TryAddSingleton<IEventAggregator, EventAggregatorService>();
        }

        /// <summary>
        /// Should register the used event aggregator. It still needs to implement the interface <see cref="IEventAggregator"/>.
        /// Override to replace the default implementation with your custom event aggregator.
        /// </summary>
        /// <param name="serviceCollection">Target to add the services to.</param>
        protected virtual void AddSynchronizationServices(IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<SampSynchronizationContext>();
        }

        /// <summary>
        /// Add services for native event handling. Override to replace the default implementation with your custom
        /// event handling.
        /// </summary>
        /// <param name="serviceCollection">Target to add the services to.</param>
        protected virtual void AddNativeEventHandling(IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<NativeTypeConverter>();
            serviceCollection.TryAddSingleton<INativeEventRegistry, NativeEventRegistry>();
        }

        /// <summary>
        /// Registers all default event collections to the service collection. Override to replace the default
        /// implementation with your custom native event collections.
        /// </summary>
        /// <param name="serviceCollection">Target to add the services to.</param>
        protected virtual void AddNativeEvents(IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<INativeEventCollectionFactory, SampEventCollectionFactory>();
            serviceCollection.AddTransient<INativeEventCollectionFactory, VehiclesEventCollectionFactory>();
            serviceCollection.AddTransient<INativeEventCollectionFactory, PlayersEventCollectionFactory>();
            serviceCollection.AddTransient<INativeEventCollectionFactory, ActorEventCollectionFactory>();
            serviceCollection.AddTransient<INativeEventCollectionFactory, ObjectsEventCollectionFactory>();
        }

        /// <summary>
        /// Registers all available natives to the service collection seperated by namespace. Override to replace the default
        /// implementation with your custom natives.
        /// </summary>
        /// <param name="serviceCollection">Target to add the services to.</param>
        protected virtual void AddNatives(IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<ISampNatives, SampNatives>();
            serviceCollection.AddTransient<IVehiclesNatives, VehiclesNatives>();
            serviceCollection.AddTransient<IPlayersNatives, PlayersNatives>();
            serviceCollection.AddTransient<IActorNatives, ActorNatives>();
            serviceCollection.AddTransient<IObjectsNatives, ObjectsNatives>();
        }

        /// <summary>
        /// Registers all included entity factories. Override to replace the default implementations with your custom
        /// factories.
        /// </summary>
        /// <param name="serviceCollection">Target to add the services to.</param>
        protected virtual void AddEntityFactories(IServiceCollection serviceCollection)
        {
            serviceCollection.TryAddTransient<IPlayerFactory, PlayerFactory>();
            serviceCollection.TryAddTransient<IVehicleFactory, VehicleFactory>();
            serviceCollection.TryAddTransient<IMainTimerFactory, MainTimerFactory>();
        }

        /// <summary>
        /// Registers all included entity pools. Override to replace the default implementations with your custom
        /// pools.
        /// </summary>
        /// <param name="serviceCollection">Target to add the services to.</param>
        protected virtual void AddEntityPools(IServiceCollection serviceCollection)
        {
            serviceCollection.TryAddSingleton<IPlayerPool, PlayerPool>();
            serviceCollection.TryAddSingleton<IVehiclePool, VehiclePool>();
        }

        /// <summary>
        /// Registers all available entity listeners. Override to replace the default implementations with your custom
        /// listeners.
        /// </summary>
        /// <param name="serviceCollection">Target to add the services to.</param>
        protected virtual void AddEntityListeners(IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<IEventListener, PlayerPoolListener>();
            serviceCollection.AddTransient<IEventListener, PlayerEventListener>();
            serviceCollection.AddTransient<IEventListener, VehicleEventListener>();
            serviceCollection.AddSingleton<IEventListener, RconEventListeners>();
            serviceCollection.AddSingleton<IEventListener, PlayerTextDrawSyncer>();
        }

        /// <summary>
        /// Registers the <see cref="IDialogHandler"/> implementation and attaches to needed events.
        /// </summary>
        /// <param name="serviceCollection">Target to add the services to.</param>
        protected virtual void AddDialogHandler(IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<IEventListener, DialogHandler>();
            serviceCollection.AddSingleton<IDialogHandler, DialogHandler>();
        }

        /// <summary>
        /// Registers all utiltiy services to use.
        /// </summary>
        /// <param name="serviceCollection">Target to add the services to.</param>
        protected virtual void AddUtilityServices(IServiceCollection serviceCollection)
        {
            serviceCollection.TryAddTransient<IVehicleMeta, VehicleMeta>();
        }

        /// <summary>
        /// Registers various helper services to use the integrated authorization.
        /// </summary>
        /// <param name="serviceCollection">Target to add the services to.</param>
        protected virtual void AddAuthorizationServices(IServiceCollection serviceCollection)
        {
            serviceCollection.TryAddSingleton<IAuthorizationFacade, AuthorizationFacade>();
            serviceCollection.AddAuthorizationCore();
        }
    }
}
