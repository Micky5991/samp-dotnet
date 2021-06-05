using System;
using System.Collections.Concurrent;
using System.Runtime.InteropServices;
using System.Threading;
using Micky5991.Samp.Net.Core.Interop;

namespace Micky5991.Samp.Net.Core.Threading
{
    public class SampSynchronizationContext : SynchronizationContext
    {
        private readonly ConcurrentQueue<(SendOrPostCallback Continuation, object State)> queue;

        private readonly int targetThreadId;

        public SampSynchronizationContext()
        {
            this.queue = new ConcurrentQueue<(SendOrPostCallback Continuation, object State)>();

            this.targetThreadId = Thread.CurrentThread.ManagedThreadId;
        }

        public override SynchronizationContext CreateCopy()
        {
            return this;
        }

        public override void Post(SendOrPostCallback d, object state)
        {
            this.queue.Enqueue((d, state));
        }

        public override void Send(SendOrPostCallback d, object state)
        {
            this.Post(d, state);
        }

        public virtual void Run()
        {
            if (Thread.CurrentThread.ManagedThreadId != this.targetThreadId)
            {
                throw new InvalidOperationException("This method can only be run from the main thread!");
            }

            // Limit to 1000 continuations, so we don't overfill it
            var taskBudget = Math.Min(this.queue.Count, 1000);
            while (taskBudget-- > 0 && this.queue.TryDequeue(out var entry))
            {
                entry.Continuation(entry.State);
            }
        }

        public virtual void Setup()
        {
            SetSynchronizationContext(this);

            Native.PluginTickDelegate callback = this.Run;
            GCHandle.Alloc(callback);

            Native.AttachTickHandler(callback);
        }
    }
}
