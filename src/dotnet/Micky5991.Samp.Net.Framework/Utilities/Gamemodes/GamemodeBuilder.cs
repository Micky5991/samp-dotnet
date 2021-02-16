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
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Micky5991.Samp.Net.Framework.Utilities.Gamemodes
{
    /// <summary>
    /// Type that registers typical core services to the service container.
    /// </summary>
    public class GamemodeBuilder
    {
        private readonly IServiceCollection serviceCollection;

        /// <summary>
        /// Initializes a new instance of the <see cref="GamemodeBuilder"/> class.
        /// </summary>
        /// <param name="serviceCollection">Service collection to use.</param>
        public GamemodeBuilder(IServiceCollection serviceCollection)
        {
            this.serviceCollection = serviceCollection;
        }

        /// <summary>
        /// Registers all core services provided by SAMP.Net.
        /// </summary>
        /// <returns>Current <see cref="GamemodeBuilder"/> instance.</returns>
        public virtual GamemodeBuilder AddCoreServices()
        {
            this.AddGamemodeStarter()
                .AddLoggerHandler()
                .AddEventAggregator()
                .AddSynchronizationServices()
                .AddNativeEventHandling()
                .AddNativeEvents()
                .AddNatives()
                .AddEntityFactories()
                .AddEntityPools()
                .AddEntityListeners()
                .AddDialogHandler()
                .AddAuthorizationServices();

            return this;
        }

        /// <summary>
        /// Runs the extension builder and registers services provided by it.
        /// </summary>
        /// <param name="extensionBuilder"><see cref="IExtensionBuilder"/> to execute and take services.</param>
        /// <returns>Current <see cref="GamemodeBuilder"/> instance.</returns>
        public virtual GamemodeBuilder AddExtensionBuilder(IExtensionBuilder extensionBuilder)
        {
            extensionBuilder.Register(this.serviceCollection);

            return this;
        }

        /// <summary>
        /// Should register the gamemode starter that should boot up the gamemode. Override to replace the default
        /// implementation with your custom starter.
        /// </summary>
        /// <returns>Current <see cref="GamemodeBuilder"/> instance.</returns>
        protected virtual GamemodeBuilder AddGamemodeStarter()
        {
            this.serviceCollection.TryAddSingleton<IGamemodeStarter, GamemodeStarter>();

            return this;
        }

        /// <summary>
        /// Should register the native samp logger that listens to all samp logs. Override to replace the default
        /// implementation with your custom listener.
        /// </summary>
        /// <returns>Current <see cref="GamemodeBuilder"/> instance.</returns>
        protected virtual GamemodeBuilder AddLoggerHandler()
        {
            this.serviceCollection.TryAddSingleton<ISampLoggerHandler, SampLoggerHandler>();

            return this;
        }

        /// <summary>
        /// Should register the used event aggregator. It still needs to implement the interface <see cref="IEventAggregator"/>.
        /// Override to replace the default implementation with your custom event aggregator.
        /// </summary>
        /// <returns>Current <see cref="GamemodeBuilder"/> instance.</returns>
        protected virtual GamemodeBuilder AddEventAggregator()
        {
            this.serviceCollection.TryAddSingleton<IEventAggregator, EventAggregatorService>();

            return this;
        }

        /// <summary>
        /// Should register the used event aggregator. It still needs to implement the interface <see cref="IEventAggregator"/>.
        /// Override to replace the default implementation with your custom event aggregator.
        /// </summary>
        /// <returns>Current <see cref="GamemodeBuilder"/> instance.</returns>
        protected virtual GamemodeBuilder AddSynchronizationServices()
        {
            this.serviceCollection.AddSingleton<SampSynchronizationContext>();

            return this;
        }

        /// <summary>
        /// Add services for native event handling. Override to replace the default implementation with your custom
        /// event handling.
        /// </summary>
        /// <returns>Current <see cref="GamemodeBuilder"/> instance.</returns>
        protected virtual GamemodeBuilder AddNativeEventHandling()
        {
            this.serviceCollection.AddTransient<NativeTypeConverter>();
            this.serviceCollection.TryAddSingleton<INativeEventRegistry, NativeEventRegistry>();

            return this;
        }

        /// <summary>
        /// Registers all default event collections to the service collection. Override to replace the default
        /// implementation with your custom native event collections.
        /// </summary>
        /// <returns>Current <see cref="GamemodeBuilder"/> instance.</returns>
        protected virtual GamemodeBuilder AddNativeEvents()
        {
            this.serviceCollection.AddTransient<INativeEventCollectionFactory, SampEventCollectionFactory>()
                .AddTransient<INativeEventCollectionFactory, VehiclesEventCollectionFactory>()
                .AddTransient<INativeEventCollectionFactory, PlayersEventCollectionFactory>()
                .AddTransient<INativeEventCollectionFactory, ActorEventCollectionFactory>()
                .AddTransient<INativeEventCollectionFactory, ObjectsEventCollectionFactory>();

            return this;
        }

        /// <summary>
        /// Registers all available natives to the service collection seperated by namespace. Override to replace the default
        /// implementation with your custom natives.
        /// </summary>
        /// <returns>Current <see cref="GamemodeBuilder"/> instance.</returns>
        protected virtual GamemodeBuilder AddNatives()
        {
            this.serviceCollection.AddTransient<ISampNatives, SampNatives>();
            this.serviceCollection.AddTransient<IVehiclesNatives, VehiclesNatives>();
            this.serviceCollection.AddTransient<IPlayersNatives, PlayersNatives>();
            this.serviceCollection.AddTransient<IActorNatives, ActorNatives>();
            this.serviceCollection.AddTransient<IObjectsNatives, ObjectsNatives>();

            return this;
        }

        /// <summary>
        /// Registers all included entity factories. Override to replace the default implementations with your custom
        /// factories.
        /// </summary>
        /// <returns>Current <see cref="GamemodeBuilder"/> instance.</returns>
        protected virtual GamemodeBuilder AddEntityFactories()
        {
            this.serviceCollection.TryAddTransient<IPlayerFactory, PlayerFactory>();
            this.serviceCollection.TryAddTransient<IVehicleFactory, VehicleFactory>();

            return this;
        }

        /// <summary>
        /// Registers all included entity pools. Override to replace the default implementations with your custom
        /// pools.
        /// </summary>
        /// <returns>Current <see cref="GamemodeBuilder"/> instance.</returns>
        protected virtual GamemodeBuilder AddEntityPools()
        {
            this.serviceCollection.TryAddSingleton<IPlayerPool, PlayerPool>();
            this.serviceCollection.TryAddSingleton<IVehiclePool, VehiclePool>();

            return this;
        }

        /// <summary>
        /// Registers all available entity listeners. Override to replace the default implementations with your custom
        /// listeners.
        /// </summary>
        /// <returns>Current <see cref="GamemodeBuilder"/> instance.</returns>
        protected virtual GamemodeBuilder AddEntityListeners()
        {
            this.serviceCollection.AddTransient<IEntityListener, PlayerPoolListener>()
                .AddTransient<IEntityListener, PlayerEventListener>()
                .AddTransient<IEntityListener, VehicleEventListener>();

            return this;
        }

        /// <summary>
        /// Registers the <see cref="IDialogHandler"/> implementation and attaches to needed events.
        /// </summary>
        /// <returns>Current <see cref="GamemodeBuilder"/> instance.</returns>
        protected virtual GamemodeBuilder AddDialogHandler()
        {
            this.serviceCollection
                .AddSingleton<IEntityListener, DialogHandler>()
                .AddSingleton<IDialogHandler, DialogHandler>();

            return this;
        }

        /// <summary>
        /// Registers various helper services to use the integrated authorization.
        /// </summary>
        /// <returns>Current <see cref="GamemodeBuilder"/> instance.</returns>
        protected virtual GamemodeBuilder AddAuthorizationServices()
        {
            this.serviceCollection
                .AddTransient<IAuthorizationFacade, AuthorizationFacade>();

            return this;
        }
    }
}
