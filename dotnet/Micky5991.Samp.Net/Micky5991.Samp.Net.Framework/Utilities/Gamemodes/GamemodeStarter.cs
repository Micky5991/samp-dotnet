using System.Collections.Generic;
using System.Threading;
using Micky5991.EventAggregator.Interfaces;
using Micky5991.Samp.Net.Core.Interfaces.Events;
using Micky5991.Samp.Net.Core.Interfaces.Logging;
using Micky5991.Samp.Net.Core.Threading;
using Micky5991.Samp.Net.Framework.Interfaces;

namespace Micky5991.Samp.Net.Framework.Utilities.Gamemodes
{
    public class GamemodeStarter : IGamemodeStarter
    {
        private readonly IEventAggregator eventAggregator;

        private readonly INativeEventRegistry eventRegistry;

        private readonly IEnumerable<INativeEventCollectionFactory> eventCollectionsFactories;

        private readonly SampSynchronizationContext synchronizationContext;

        private readonly ISampLoggerHandler sampLoggerHandler;

        public GamemodeStarter(
            IEventAggregator eventAggregator,
            INativeEventRegistry eventRegistry,
            IEnumerable<INativeEventCollectionFactory> eventCollectionsFactories,
            SampSynchronizationContext synchronizationContext,
            ISampLoggerHandler sampLoggerHandler)
        {
            this.eventAggregator = eventAggregator;
            this.eventRegistry = eventRegistry;
            this.eventCollectionsFactories = eventCollectionsFactories;
            this.synchronizationContext = synchronizationContext;
            this.sampLoggerHandler = sampLoggerHandler;
        }

        public virtual IGamemodeStarter Start()
        {
            this.StartSynchronizationContext();
            this.StartEventAggregator();
            this.StartEvents();

            return this;
        }

        public virtual IGamemodeStarter StartLogRedirection()
        {
            this.sampLoggerHandler.Attach();

            return this;
        }

        private void StartSynchronizationContext()
        {
            this.synchronizationContext.Setup();
        }

        protected void StartEventAggregator()
        {
            this.eventAggregator.SetMainThreadSynchronizationContext(SynchronizationContext.Current);
        }

        protected virtual void StartEvents()
        {
            this.eventRegistry.AttachEventInvoker();

            foreach (var nativeEventCollectionFactory in this.eventCollectionsFactories)
            {
                this.eventRegistry.RegisterEvents(nativeEventCollectionFactory.Build());
            }
        }
    }
}
