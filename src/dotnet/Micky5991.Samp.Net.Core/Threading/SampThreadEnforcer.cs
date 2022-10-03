using System;
using System.Threading;
using Micky5991.Samp.Net.Core.Exceptions;
using Micky5991.Samp.Net.Core.Interfaces.Interop;
using Microsoft.Extensions.Logging;

namespace Micky5991.Samp.Net.Core.Threading
{
    public class SampThreadEnforcer : ISampThreadEnforcer
    {
        private readonly ILogger<SampThreadEnforcer> logger;

        public SampThreadEnforcer(ILogger<SampThreadEnforcer> logger)
        {
            this.logger = logger;
        }

        public int? MainThread { get; private set; }

        public void SetMainThread()
        {
            if (this.MainThread != null)
            {
                throw new InvalidOperationException($"The main thread has already been set to {this.MainThread}");
            }

            this.MainThread = Thread.CurrentThread.ManagedThreadId;
        }

        public void EnforceMainThread(string callerMemberName = "")
        {
            if (this.MainThread == null)
            {
                this.logger.LogError($"{nameof(ISampThreadEnforcer)} has not been properly set up with {nameof(this.SetMainThread)}. Aborting call.");
                throw new InvalidThreadException($"{nameof(ISampThreadEnforcer)} has not been properly set up with {nameof(this.SetMainThread)}. Aborting call.");
            }

            if (Thread.CurrentThread.ManagedThreadId == this.MainThread)
            {
                return;
            }

            this.logger.LogError($"{callerMemberName} has been called outside the main thread, aborting call!");
            throw new InvalidThreadException($"{callerMemberName} has been called outside the main thread");
        }
    }
}
