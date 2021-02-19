using System;
using System.Collections.Generic;
using Dawn;
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

        private readonly List<ISampExtension> extensions;

        /// <summary>
        /// Initializes a new instance of the <see cref="GamemodeBuilder"/> class.
        /// </summary>
        /// <param name="serviceCollection">Service collection to use.</param>
        public GamemodeBuilder(IServiceCollection serviceCollection)
        {
            this.serviceCollection = serviceCollection;

            this.extensions = new List<ISampExtension>();
        }

        /// <summary>
        /// Adds an extension to the <see cref="GamemodeBuilder"/>.
        /// </summary>
        /// <param name="extension">Extension-instance to add.</param>
        /// <param name="configure">Optional configuration for this extension to avoid double instantiation.</param>
        /// <typeparam name="T">Actual type of the extension.</typeparam>
        /// <returns>Current <see cref="GamemodeBuilder"/> instance.</returns>
        public virtual GamemodeBuilder AddExtension<T>(T extension, Action<T>? configure = null)
            where T : class, ISampExtension
        {
            Guard.Argument(extension, nameof(extension)).NotNull();

            configure?.Invoke(extension);

            extension.RegisterServices(this.serviceCollection);

            this.extensions.Add(extension);

            return this;
        }

        /// <summary>
        /// Instantiates the extension <typeparamref name="T"/> with the parameterless-constructor and executes <see cref="AddExtension{T}(T,System.Action{T}?)"/>.
        /// </summary>
        /// <param name="configure">Actions to apply to the extension <paramref name="configure"/>.</param>
        /// <typeparam name="T">Actual type of the extension.</typeparam>
        /// <returns>Current <see cref="GamemodeBuilder"/> instance.</returns>
        public virtual GamemodeBuilder AddExtension<T>(Action<T>? configure = null)
            where T : class, ISampExtension, new()
        {
            return this.AddExtension(new T(), configure);
        }

        /// <summary>
        /// Registers all core services provided by SAMP.Net.
        /// </summary>
        /// <returns>Current <see cref="GamemodeBuilder"/> instance.</returns>
        public virtual GamemodeBuilder AddAllServices()
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
        /// Should register the gamemode starter that should boot up the gamemode. Override to replace the default
        /// implementation with your custom starter.
        /// </summary>
        /// <returns>Current <see cref="GamemodeBuilder"/> instance.</returns>
        public virtual GamemodeBuilder AddGamemodeStarter()
        {
            this.serviceCollection.TryAddSingleton<IGamemodeStarter, GamemodeStarter>();

            return this;
        }

        /// <summary>
        /// Should register the native samp logger that listens to all samp logs. Override to replace the default
        /// implementation with your custom listener.
        /// </summary>
        /// <returns>Current <see cref="GamemodeBuilder"/> instance.</returns>
        public virtual GamemodeBuilder AddLoggerHandler()
        {
            this.serviceCollection.TryAddSingleton<ISampLoggerHandler, SampLoggerHandler>();

            return this;
        }

        /// <summary>
        /// Should register the used event aggregator. It still needs to implement the interface <see cref="IEventAggregator"/>.
        /// Override to replace the default implementation with your custom event aggregator.
        /// </summary>
        /// <returns>Current <see cref="GamemodeBuilder"/> instance.</returns>
        public virtual GamemodeBuilder AddEventAggregator()
        {
            this.serviceCollection.TryAddSingleton<IEventAggregator, EventAggregatorService>();

            return this;
        }

        /// <summary>
        /// Should register the used event aggregator. It still needs to implement the interface <see cref="IEventAggregator"/>.
        /// Override to replace the default implementation with your custom event aggregator.
        /// </summary>
        /// <returns>Current <see cref="GamemodeBuilder"/> instance.</returns>
        public virtual GamemodeBuilder AddSynchronizationServices()
        {
            this.serviceCollection.AddSingleton<SampSynchronizationContext>();

            return this;
        }

        /// <summary>
        /// Add services for native event handling. Override to replace the default implementation with your custom
        /// event handling.
        /// </summary>
        /// <returns>Current <see cref="GamemodeBuilder"/> instance.</returns>
        public virtual GamemodeBuilder AddNativeEventHandling()
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
        public virtual GamemodeBuilder AddNativeEvents()
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
        public virtual GamemodeBuilder AddNatives()
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
        public virtual GamemodeBuilder AddEntityFactories()
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
        public virtual GamemodeBuilder AddEntityPools()
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
        public virtual GamemodeBuilder AddEntityListeners()
        {
            this.serviceCollection.AddTransient<IEntityListener, PlayerPoolListener>()
                .AddTransient<IEntityListener, PlayerEventListener>()
                .AddTransient<IEntityListener, VehicleEventListener>()
                .AddSingleton<IEntityListener, RconEventListeners>();

            return this;
        }

        /// <summary>
        /// Registers the <see cref="IDialogHandler"/> implementation and attaches to needed events.
        /// </summary>
        /// <returns>Current <see cref="GamemodeBuilder"/> instance.</returns>
        public virtual GamemodeBuilder AddDialogHandler()
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
        public virtual GamemodeBuilder AddAuthorizationServices()
        {
            this.serviceCollection
                .AddSingleton<IAuthorizationFacade, AuthorizationFacade>()
                .AddAuthorizationCore(
                                      config =>
                                      {
                                          foreach (var extension in this.extensions)
                                          {
                                            extension.ConfigureAuthorization(config);
                                          }
                                      });

            return this;
        }
    }
}
