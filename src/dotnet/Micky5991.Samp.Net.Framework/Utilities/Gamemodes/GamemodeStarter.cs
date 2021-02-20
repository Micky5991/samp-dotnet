using System.Collections.Generic;
using System.Threading;
using Micky5991.EventAggregator.Interfaces;
using Micky5991.Samp.Net.Core.Interfaces.Events;
using Micky5991.Samp.Net.Core.Interfaces.Logging;
using Micky5991.Samp.Net.Core.Threading;
using Micky5991.Samp.Net.Framework.Interfaces;
using Micky5991.Samp.Net.Framework.Options;
using Microsoft.Extensions.Options;

namespace Micky5991.Samp.Net.Framework.Utilities.Gamemodes
{
    /// <inheritdoc />
    public class GamemodeStarter : IGamemodeStarter
    {
        private readonly IEventAggregator eventAggregator;

        private readonly INativeEventRegistry eventRegistry;

        private readonly IEnumerable<INativeEventCollectionFactory> eventCollectionsFactories;

        private readonly SampSynchronizationContext synchronizationContext;

        private readonly ISampLoggerHandler sampLoggerHandler;

        private readonly IEnumerable<IEntityListener> entityListeners;

        private readonly IEnumerable<ISampExtensionStarter> extensions;

        private readonly GamemodeOptions gamemodeOptions;

        /// <summary>
        /// Initializes a new instance of the <see cref="GamemodeStarter"/> class.
        /// </summary>
        /// <param name="eventAggregator">Aggregator instance to start.</param>
        /// <param name="eventRegistry">Registry instance to start.</param>
        /// <param name="eventCollectionsFactories">List of factories which provide information how native events are formatted.</param>
        /// <param name="synchronizationContext">Synchronization context that handles main thread tasks.</param>
        /// <param name="sampLoggerHandler">Handler that bootstraps samp server log redirection.</param>
        /// <param name="entityListeners">List of entity listeners to activate.</param>
        /// <param name="extensions">List of extensions of this gamemode.</param>
        /// <param name="gamemodeOptions">Options needed to configure the gamemode behavior.</param>
        public GamemodeStarter(
            IEventAggregator eventAggregator,
            INativeEventRegistry eventRegistry,
            IEnumerable<INativeEventCollectionFactory> eventCollectionsFactories,
            SampSynchronizationContext synchronizationContext,
            ISampLoggerHandler sampLoggerHandler,
            IEnumerable<IEntityListener> entityListeners,
            IEnumerable<ISampExtensionStarter> extensions,
            IOptions<GamemodeOptions> gamemodeOptions)
        {
            this.eventAggregator = eventAggregator;
            this.eventRegistry = eventRegistry;
            this.eventCollectionsFactories = eventCollectionsFactories;
            this.synchronizationContext = synchronizationContext;
            this.sampLoggerHandler = sampLoggerHandler;
            this.entityListeners = entityListeners;
            this.extensions = extensions;
            this.gamemodeOptions = gamemodeOptions.Value;
        }

        /// <inheritdoc />
        public virtual IGamemodeStarter Start()
        {
            if (this.gamemodeOptions.LogRedirection)
            {
                this.StartLogRedirection();
            }

            this.StartSynchronizationContext();
            this.StartEventAggregator();
            this.StartEvents();
            this.StartEntityListeners();
            this.StartExtensions();

            return this;
        }

        /// <summary>
        /// Enables log redirection so you can handle any original samp server events in SAMP.Net.
        /// </summary>
        protected virtual void StartLogRedirection()
        {
            this.sampLoggerHandler.Attach();
        }

        /// <summary>
        /// Starts the event aggregator and sets the main thread.
        /// </summary>
        protected void StartEventAggregator()
        {
            this.eventAggregator.SetMainThreadSynchronizationContext(SynchronizationContext.Current);
        }

        /// <summary>
        /// Registers all events and stores format information for each event.
        /// </summary>
        protected virtual void StartEvents()
        {
            this.eventRegistry.AttachEventInvoker();

            foreach (var nativeEventCollectionFactory in this.eventCollectionsFactories)
            {
                this.eventRegistry.RegisterEvents(nativeEventCollectionFactory.Build());
            }
        }

        /// <summary>
        /// Attach entity listeners.
        /// </summary>
        protected virtual void StartEntityListeners()
        {
            foreach (var entityListener in this.entityListeners)
            {
                entityListener.Attach();
            }
        }

        /// <summary>
        /// Starts the services of every extension.
        /// </summary>
        protected virtual void StartExtensions()
        {
            foreach (var extensionStarter in this.extensions)
            {
                extensionStarter.Start();
            }
        }

        private void StartSynchronizationContext()
        {
            this.synchronizationContext.Setup();
        }
    }
}
