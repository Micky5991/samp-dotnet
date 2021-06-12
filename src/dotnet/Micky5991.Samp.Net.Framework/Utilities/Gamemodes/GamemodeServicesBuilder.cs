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
using Micky5991.Samp.Net.Framework.Services;
using Micky5991.Samp.Net.Framework.Services.Facades;
using Micky5991.Samp.Net.Framework.Services.Syncer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Micky5991.Samp.Net.Framework.Utilities.Gamemodes
{
    /// <summary>
    /// Type that registers typical core services to the service container.
    /// </summary>
    public class GamemodeServicesBuilder
    {
        /// <summary>
        /// Registers all core services provided by SAMP.Net.
        /// </summary>
        /// <param name="serviceCollection">Target to add the services to.</param>
        /// <returns>Current <see cref="GamemodeServicesBuilder"/> instance.</returns>
        public virtual GamemodeServicesBuilder AddAllServices(IServiceCollection serviceCollection)
        {
            Guard.Argument(serviceCollection, nameof(serviceCollection)).NotNull();

            this.AddCoreServices(serviceCollection)
                .AddGamemodeStarter(serviceCollection)
                .AddLoggerHandler(serviceCollection)
                .AddEventAggregator(serviceCollection)
                .AddSynchronizationServices(serviceCollection)
                .AddNativeEventHandling(serviceCollection)
                .AddNativeEvents(serviceCollection)
                .AddNatives(serviceCollection)
                .AddEntityFactories(serviceCollection)
                .AddEntityPools(serviceCollection)
                .AddEntityListeners(serviceCollection)
                .AddDialogHandler(serviceCollection)
                .AddUtilityServices(serviceCollection)
                .AddAuthorizationServices(serviceCollection);

            return this;
        }

        /// <summary>
        /// Should register the core services needed for basic operation of the framework.
        /// </summary>
        /// <param name="serviceCollection">Target to add the services to.</param>
        /// <returns>Current <see cref="GamemodeServicesBuilder"/> instance.</returns>
        protected GamemodeServicesBuilder AddCoreServices(IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<ISampThreadEnforcer, SampThreadEnforcer>();

            return this;
        }

        /// <summary>
        /// Should register the gamemode starter that should boot up the gamemode. Override to replace the default
        /// implementation with your custom starter.
        /// </summary>
        /// <param name="serviceCollection">Target to add the services to.</param>
        /// <returns>Current <see cref="GamemodeServicesBuilder"/> instance.</returns>
        protected virtual GamemodeServicesBuilder AddGamemodeStarter(IServiceCollection serviceCollection)
        {
            serviceCollection.TryAddSingleton<IGamemodeStarter, GamemodeStarter>();

            return this;
        }

        /// <summary>
        /// Should register the native samp logger that listens to all samp logs. Override to replace the default
        /// implementation with your custom listener.
        /// </summary>
        /// <param name="serviceCollection">Target to add the services to.</param>
        /// <returns>Current <see cref="GamemodeServicesBuilder"/> instance.</returns>
        protected virtual GamemodeServicesBuilder AddLoggerHandler(IServiceCollection serviceCollection)
        {
            serviceCollection.TryAddSingleton<ISampLoggerHandler, SampLoggerHandler>();

            return this;
        }

        /// <summary>
        /// Should register the used event aggregator. It still needs to implement the interface <see cref="IEventAggregator"/>.
        /// Override to replace the default implementation with your custom event aggregator.
        /// </summary>
        /// <param name="serviceCollection">Target to add the services to.</param>
        /// <returns>Current <see cref="GamemodeServicesBuilder"/> instance.</returns>
        protected virtual GamemodeServicesBuilder AddEventAggregator(IServiceCollection serviceCollection)
        {
            serviceCollection.TryAddSingleton<IEventAggregator, EventAggregatorService>();

            return this;
        }

        /// <summary>
        /// Should register the used event aggregator. It still needs to implement the interface <see cref="IEventAggregator"/>.
        /// Override to replace the default implementation with your custom event aggregator.
        /// </summary>
        /// <param name="serviceCollection">Target to add the services to.</param>
        /// <returns>Current <see cref="GamemodeServicesBuilder"/> instance.</returns>
        protected virtual GamemodeServicesBuilder AddSynchronizationServices(IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<SampSynchronizationContext>();

            return this;
        }

        /// <summary>
        /// Add services for native event handling. Override to replace the default implementation with your custom
        /// event handling.
        /// </summary>
        /// <param name="serviceCollection">Target to add the services to.</param>
        /// <returns>Current <see cref="GamemodeServicesBuilder"/> instance.</returns>
        protected virtual GamemodeServicesBuilder AddNativeEventHandling(IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<NativeTypeConverter>();
            serviceCollection.TryAddSingleton<INativeEventRegistry, NativeEventRegistry>();

            return this;
        }

        /// <summary>
        /// Registers all default event collections to the service collection. Override to replace the default
        /// implementation with your custom native event collections.
        /// </summary>
        /// <param name="serviceCollection">Target to add the services to.</param>
        /// <returns>Current <see cref="GamemodeServicesBuilder"/> instance.</returns>
        protected virtual GamemodeServicesBuilder AddNativeEvents(IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<INativeEventCollectionFactory, SampEventCollectionFactory>();
            serviceCollection.AddTransient<INativeEventCollectionFactory, VehiclesEventCollectionFactory>();
            serviceCollection.AddTransient<INativeEventCollectionFactory, PlayersEventCollectionFactory>();
            serviceCollection.AddTransient<INativeEventCollectionFactory, ActorEventCollectionFactory>();
            serviceCollection.AddTransient<INativeEventCollectionFactory, ObjectsEventCollectionFactory>();

            return this;
        }

        /// <summary>
        /// Registers all available natives to the service collection seperated by namespace. Override to replace the default
        /// implementation with your custom natives.
        /// </summary>
        /// <param name="serviceCollection">Target to add the services to.</param>
        /// <returns>Current <see cref="GamemodeServicesBuilder"/> instance.</returns>
        protected virtual GamemodeServicesBuilder AddNatives(IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<ISampNatives, SampNatives>();
            serviceCollection.AddTransient<IVehiclesNatives, VehiclesNatives>();
            serviceCollection.AddTransient<IPlayersNatives, PlayersNatives>();
            serviceCollection.AddTransient<IActorNatives, ActorNatives>();
            serviceCollection.AddTransient<IObjectsNatives, ObjectsNatives>();

            return this;
        }

        /// <summary>
        /// Registers all included entity factories. Override to replace the default implementations with your custom
        /// factories.
        /// </summary>
        /// <param name="serviceCollection">Target to add the services to.</param>
        /// <returns>Current <see cref="GamemodeServicesBuilder"/> instance.</returns>
        protected virtual GamemodeServicesBuilder AddEntityFactories(IServiceCollection serviceCollection)
        {
            serviceCollection.TryAddTransient<IPlayerFactory, PlayerFactory>();
            serviceCollection.TryAddTransient<IVehicleFactory, VehicleFactory>();
            serviceCollection.TryAddTransient<IMainTimerFactory, MainTimerFactory>();

            return this;
        }

        /// <summary>
        /// Registers all included entity pools. Override to replace the default implementations with your custom
        /// pools.
        /// </summary>
        /// <param name="serviceCollection">Target to add the services to.</param>
        /// <returns>Current <see cref="GamemodeServicesBuilder"/> instance.</returns>
        protected virtual GamemodeServicesBuilder AddEntityPools(IServiceCollection serviceCollection)
        {
            serviceCollection.TryAddSingleton<IPlayerPool, PlayerPool>();
            serviceCollection.TryAddSingleton<IVehiclePool, VehiclePool>();

            return this;
        }

        /// <summary>
        /// Registers all available entity listeners. Override to replace the default implementations with your custom
        /// listeners.
        /// </summary>
        /// <param name="serviceCollection">Target to add the services to.</param>
        /// <returns>Current <see cref="GamemodeServicesBuilder"/> instance.</returns>
        protected virtual GamemodeServicesBuilder AddEntityListeners(IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<IEntityListener, PlayerPoolListener>();
            serviceCollection.AddTransient<IEntityListener, PlayerEventListener>();
            serviceCollection.AddTransient<IEntityListener, VehicleEventListener>();
            serviceCollection.AddSingleton<IEntityListener, RconEventListeners>();
            serviceCollection.AddSingleton<IEntityListener, PlayerTextDrawSyncer>();

            return this;
        }

        /// <summary>
        /// Registers the <see cref="IDialogHandler"/> implementation and attaches to needed events.
        /// </summary>
        /// <param name="serviceCollection">Target to add the services to.</param>
        /// <returns>Current <see cref="GamemodeServicesBuilder"/> instance.</returns>
        protected virtual GamemodeServicesBuilder AddDialogHandler(IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<IEntityListener, DialogHandler>();
            serviceCollection.AddSingleton<IDialogHandler, DialogHandler>();

            return this;
        }

        /// <summary>
        /// Registers all utiltiy services to use.
        /// </summary>
        /// <param name="serviceCollection">Target to add the services to.</param>
        /// <returns>Current <see cref="GamemodeServicesBuilder"/> instance.</returns>
        protected virtual GamemodeServicesBuilder AddUtilityServices(IServiceCollection serviceCollection)
        {
            serviceCollection.TryAddTransient<IVehicleMeta, VehicleMeta>();

            return this;
        }

        /// <summary>
        /// Registers various helper services to use the integrated authorization.
        /// </summary>
        /// <param name="serviceCollection">Target to add the services to.</param>
        /// <returns>Current <see cref="GamemodeServicesBuilder"/> instance.</returns>
        protected virtual GamemodeServicesBuilder AddAuthorizationServices(IServiceCollection serviceCollection)
        {
            serviceCollection.TryAddSingleton<IAuthorizationFacade, AuthorizationFacade>();
            serviceCollection.AddAuthorizationCore();

            return this;
        }
    }
}
