// <copyright file="CommandExtensionLifecycle.cs" company="Micky5991">
// Copyright (c) Micky5991. All rights reserved.
// </copyright>

using System;
using System.Threading;
using System.Threading.Tasks;
using Micky5991.Samp.Net.Commands.Interfaces;
using Microsoft.Extensions.Hosting;

namespace Micky5991.Samp.Net.Commands
{
    /// <inheritdoc />
    public class CommandExtensionLifecycle : IHostedService
    {
        private readonly ICommandService commandService;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandExtensionLifecycle"/> class.
        /// </summary>
        /// <param name="commandService"><see cref="ICommandService"/> to get from DI.</param>
        public CommandExtensionLifecycle(ICommandService commandService)
        {
            this.commandService = commandService;
        }

        /// <inheritdoc />
        public Task StartAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine($"STart {nameof(CommandExtensionLifecycle)}");

            this.commandService.Start();

            return Task.CompletedTask;
        }

        /// <inheritdoc />
        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
