// <copyright file="ServerLifecycleHostedService.cs" company="Micky5991">
// Copyright (c) Micky5991. All rights reserved.
// </copyright>

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Micky5991.Samp.Net.Framework.Utilities.Gamemodes
{
    public class ServerLifecycleHostedService : IHostedService
    {
        private readonly ILogger<ServerLifecycleHostedService> logger;

        private readonly IHostApplicationLifetime appLifetime;

        private readonly IHost host;

        public ServerLifecycleHostedService(
            ILogger<ServerLifecycleHostedService> logger,
            IHostApplicationLifetime appLifetime,
            IHost host)
        {
            this.logger = logger;
            this.appLifetime = appLifetime;
            this.host = host;
        }

        /// <inheritdoc />
        public Task StartAsync(CancellationToken cancellationToken)
        {
            this.appLifetime.ApplicationStopping.Register(this.OnApplicationStopping);
            this.appLifetime.ApplicationStopped.Register(this.OnApplicationStopped);

            return Task.CompletedTask;
        }

        /// <inheritdoc />
        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        private void OnApplicationStopped()
        {
            this.logger.LogInformation("Application stopped, exiting host.");

            Environment.Exit(0);
        }

        private void OnApplicationStopping()
        {
            this.host.StopAsync();
        }
    }
}
