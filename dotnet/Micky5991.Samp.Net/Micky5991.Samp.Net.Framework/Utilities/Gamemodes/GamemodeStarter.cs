using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using Micky5991.EventAggregator.Interfaces;
using Micky5991.Samp.Net.Core.Interfaces.Events;
using Micky5991.Samp.Net.Core.Interop;
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

        public GamemodeStarter(
            IEventAggregator eventAggregator,
            INativeEventRegistry eventRegistry,
            IEnumerable<INativeEventCollectionFactory> eventCollectionsFactories,
            SampSynchronizationContext synchronizationContext)
        {
            this.eventAggregator = eventAggregator;
            this.eventRegistry = eventRegistry;
            this.eventCollectionsFactories = eventCollectionsFactories;
            this.synchronizationContext = synchronizationContext;
        }

        public virtual void Start()
        {
            this.StartSynchronizationContext();
            this.StartEventAggregator();
            this.StartEvents();
        }

        private void StartSynchronizationContext()
        {
            SynchronizationContext.SetSynchronizationContext(this.synchronizationContext);

            Native.PluginTickDelegate callback = this.synchronizationContext.Run;
            GCHandle.Alloc(callback);

            Native.AttachTickHandler(callback);
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
