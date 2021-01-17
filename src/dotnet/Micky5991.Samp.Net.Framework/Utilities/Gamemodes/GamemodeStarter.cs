using System.Collections.Generic;
using System.Threading;
using Micky5991.EventAggregator.Interfaces;
using Micky5991.Samp.Net.Core.Interfaces.Events;
using Micky5991.Samp.Net.Core.Interfaces.Logging;
using Micky5991.Samp.Net.Core.Threading;
using Micky5991.Samp.Net.Framework.Interfaces;

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

        /// <summary>
        /// Initializes a new instance of the <see cref="GamemodeStarter"/> class.
        /// </summary>
        /// <param name="eventAggregator">Aggregator instance to start.</param>
        /// <param name="eventRegistry">Registry instance to start.</param>
        /// <param name="eventCollectionsFactories">List of factories which provide information how native events are formatted.</param>
        /// <param name="synchronizationContext">Synchronization context that handles main thread tasks.</param>
        /// <param name="sampLoggerHandler">Handler that bootstraps samp server log redirection.</param>
        /// <param name="entityListeners">List of entity listeners to activate.</param>
        public GamemodeStarter(
            IEventAggregator eventAggregator,
            INativeEventRegistry eventRegistry,
            IEnumerable<INativeEventCollectionFactory> eventCollectionsFactories,
            SampSynchronizationContext synchronizationContext,
            ISampLoggerHandler sampLoggerHandler,
            IEnumerable<IEntityListener> entityListeners)
        {
            this.eventAggregator = eventAggregator;
            this.eventRegistry = eventRegistry;
            this.eventCollectionsFactories = eventCollectionsFactories;
            this.synchronizationContext = synchronizationContext;
            this.sampLoggerHandler = sampLoggerHandler;
            this.entityListeners = entityListeners;
        }

        /// <inheritdoc />
        public virtual IGamemodeStarter Start()
        {
            this.StartSynchronizationContext();
            this.StartEventAggregator();
            this.StartEvents();
            this.StartEntityListeners();

            return this;
        }

        /// <inheritdoc />
        public virtual IGamemodeStarter StartLogRedirection()
        {
            this.sampLoggerHandler.Attach();

            return this;
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

        private void StartSynchronizationContext()
        {
            this.synchronizationContext.Setup();
        }
    }
}
