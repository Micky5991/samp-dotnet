using System.Collections.Generic;
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

        private readonly IEnumerable<INativeEventCollection> eventCollections;

        private readonly SampSynchronizationContext synchronizationContext;

        public GamemodeStarter(
            IEventAggregator eventAggregator,
            INativeEventRegistry eventRegistry,
            IEnumerable<INativeEventCollection> eventCollections,
            SampSynchronizationContext synchronizationContext)
        {
            this.eventAggregator = eventAggregator;
            this.eventRegistry = eventRegistry;
            this.eventCollections = eventCollections;
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
            this.eventRegistry.RegisterEvents(this.eventCollections);
        }
    }
}
