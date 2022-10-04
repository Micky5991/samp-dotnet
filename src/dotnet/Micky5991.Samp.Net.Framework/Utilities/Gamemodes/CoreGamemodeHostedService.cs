// <copyright file="CoreGamemodeBuilder.Start.cs" company="Micky5991">
// Copyright (c) Micky5991. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Micky5991.EventAggregator.Interfaces;
using Micky5991.Samp.Net.Core.Interfaces.Events;
using Micky5991.Samp.Net.Core.Interfaces.Interop;
using Micky5991.Samp.Net.Core.Interfaces.Logging;
using Micky5991.Samp.Net.Core.Threading;
using Micky5991.Samp.Net.Framework.Interfaces;
using Micky5991.Samp.Net.Framework.Options;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace Micky5991.Samp.Net.Framework.Utilities.Gamemodes
{
    /// <inheritdoc cref="CoreHostBuilder" />
    public class CoreGamemodeHostedService : IHostedService
    {
        private readonly SampNetOptions sampnetOptions;

        private readonly ISampLoggerHandler loggerHandler;

        private readonly ISampThreadEnforcer sampThreadEnforcer;

        private readonly INativeEventRegistry nativeEventRegistry;

        private readonly IEventAggregator eventAggregator;

        private readonly IEnumerable<INativeEventCollectionFactory> nativeEventCollectionFactories;

        private readonly IEnumerable<IEventListener> eventListeners;

        /// <summary>
        /// Initializes a new instance of the <see cref="CoreGamemodeHostedService"/> class.
        /// </summary>
        /// <param name="sampnetOptions"><see cref="SampNetOptions"/> to fetch from DI.</param>
        /// <param name="loggerHandler"><see cref="ISampLoggerHandler"/> to fetch from DI.</param>
        /// <param name="sampThreadEnforcer"><see cref="ISampThreadEnforcer"/> to fetch from DI.</param>
        /// <param name="nativeEventRegistry"><see cref="INativeEventRegistry"/> to fetch from DI.</param>
        /// <param name="eventAggregator"><see cref="IEventAggregator"/> to fetch from DI.</param>
        /// <param name="nativeEventCollectionFactories">List of <see cref="INativeEventCollectionFactory"/> to fetch from DI.</param>
        /// <param name="eventListeners">List of <see cref="IEventListener"/> to fetch from DI.</param>
        public CoreGamemodeHostedService(
            IOptions<SampNetOptions> sampnetOptions,
            ISampLoggerHandler loggerHandler,
            ISampThreadEnforcer sampThreadEnforcer,
            INativeEventRegistry nativeEventRegistry,
            IEventAggregator eventAggregator,
            IEnumerable<INativeEventCollectionFactory> nativeEventCollectionFactories,
            IEnumerable<IEventListener> eventListeners)
        {
            this.sampnetOptions = sampnetOptions.Value;
            this.loggerHandler = loggerHandler;
            this.sampThreadEnforcer = sampThreadEnforcer;
            this.nativeEventRegistry = nativeEventRegistry;
            this.eventAggregator = eventAggregator;
            this.nativeEventCollectionFactories = nativeEventCollectionFactories;
            this.eventListeners = eventListeners;
        }

        /// <inheritdoc />
        public Task StartAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine($"STart {nameof(CoreGamemodeHostedService)}");

            if (this.sampnetOptions.LogRedirection)
            {
                this.StartLogRedirection(this.loggerHandler);
            }

            this.StartThreadEnforcer(this.sampThreadEnforcer);
            this.StartEventAggregator(this.eventAggregator);
            this.StartEvents(this.nativeEventRegistry, this.nativeEventCollectionFactories);
            this.StartEventListeners(this.eventListeners);

            return Task.CompletedTask;
        }

        /// <inheritdoc />
        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        /// <summary>
        /// Enables log redirection so you can handle any original samp server events in SAMP.Net.
        /// </summary>
        /// <param name="loggerHandler"><see cref="ISampLoggerHandler"/> to attach.</param>
        protected virtual void StartLogRedirection(ISampLoggerHandler loggerHandler)
        {
            loggerHandler.Attach();
        }

        /// <summary>
        /// Starts the event aggregator and sets the main thread.
        /// </summary>
        /// <param name="eventAggregator"><see cref="IEventAggregator"/> instance to set main thread on.</param>
        protected void StartEventAggregator(IEventAggregator eventAggregator)
        {
            eventAggregator.SetMainThreadSynchronizationContext(SynchronizationContext.Current);
        }

        /// <summary>
        /// Registers all events and stores format information for each event.
        /// </summary>
        /// <param name="nativeEventRegistry"><see cref="INativeEventRegistry"/> instance to register all found events to.</param>
        /// <param name="eventCollectionFactory">List of <see cref="INativeEventCollectionFactory"/> to get all events from.</param>
        protected virtual void StartEvents(INativeEventRegistry nativeEventRegistry, IEnumerable<INativeEventCollectionFactory> eventCollectionFactory)
        {
            nativeEventRegistry.AttachEventInvoker();

            foreach (var nativeEventCollectionFactory in eventCollectionFactory)
            {
                nativeEventRegistry.RegisterEvents(nativeEventCollectionFactory.Build());
            }
        }

        /// <summary>
        /// Attach entity listeners.
        /// </summary>
        /// <param name="eventListeners">List of <see cref="IEventListener"/> instances to start.</param>
        protected virtual void StartEventListeners(IEnumerable<IEventListener> eventListeners)
        {
            foreach (var entityListener in eventListeners)
            {
                entityListener.Attach();
            }
        }

        private void StartThreadEnforcer(ISampThreadEnforcer threadEnforcer)
        {
            threadEnforcer.SetMainThread();
        }
    }
}
