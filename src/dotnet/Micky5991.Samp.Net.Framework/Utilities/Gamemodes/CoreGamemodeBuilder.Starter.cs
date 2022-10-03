// <copyright file="CoreGamemodeBuilder.Start.cs" company="Micky5991">
// Copyright (c) Micky5991. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Threading;
using Micky5991.EventAggregator.Interfaces;
using Micky5991.Samp.Net.Core.Interfaces.Events;
using Micky5991.Samp.Net.Core.Interfaces.Interop;
using Micky5991.Samp.Net.Core.Interfaces.Logging;
using Micky5991.Samp.Net.Core.Threading;
using Micky5991.Samp.Net.Framework.Interfaces;
using Micky5991.Samp.Net.Framework.Options;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Micky5991.Samp.Net.Framework.Utilities.Gamemodes
{
    /// <inheritdoc cref="CoreGamemodeBuilder" />
    public partial class CoreGamemodeBuilder
    {
        /// <inheritdoc />
        public override void Start(IServiceProvider serviceProvider)
        {
            var sampNetOptions = serviceProvider.GetRequiredService<IOptions<SampNetOptions>>().Value;

            if (sampNetOptions.LogRedirection)
            {
                this.StartLogRedirection(serviceProvider.GetRequiredService<ISampLoggerHandler>());
            }

            this.StartThreadEnforcer(serviceProvider.GetRequiredService<ISampThreadEnforcer>());
            this.StartSynchronizationContext(serviceProvider.GetRequiredService<SampSynchronizationContext>());
            this.StartEventAggregator(serviceProvider.GetRequiredService<IEventAggregator>());
            this.StartEvents(
                serviceProvider.GetRequiredService<INativeEventRegistry>(),
                serviceProvider.GetServices<INativeEventCollectionFactory>());
            this.StartEventListeners(serviceProvider.GetServices<IEventListener>());
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

        private void StartSynchronizationContext(SampSynchronizationContext synchronizationContext)
        {
            synchronizationContext.Setup();
        }
    }
}
