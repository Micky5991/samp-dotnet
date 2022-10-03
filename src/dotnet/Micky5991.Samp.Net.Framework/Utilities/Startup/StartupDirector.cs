// <copyright file="StartupDirector.cs" company="Micky5991">
// Copyright (c) Micky5991. All rights reserved.
// </copyright>

using System;
using System.Collections.Immutable;
using System.Linq;
using Micky5991.Samp.Net.Framework.Interfaces.Startup;
using Microsoft.Extensions.DependencyInjection;

namespace Micky5991.Samp.Net.Framework.Utilities.Startup
{
    /// <inheritdoc />
    public class StartupDirector : IStartupDirector
    {
        private IServerBuilder serverBuilder;

        private IImmutableList<IGamemodeBuilder> builders = Array.Empty<IGamemodeBuilder>().ToImmutableList();
        private IImmutableList<IGamemodeBuilder> serverAndGamemodeBuilders = Array.Empty<IGamemodeBuilder>().ToImmutableList();

        private IServiceProvider? serviceProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="StartupDirector"/> class.
        /// </summary>
        /// <param name="serverBuilder">Initial <see cref="IServerBuilder"/> instance.</param>
        public StartupDirector(IServerBuilder serverBuilder)
        {
            this.serverBuilder = serverBuilder;
            this.UpdateServerAndGamemodeBuilders();
        }

        /// <inheritdoc />
        public virtual IStartupDirector AddGamemodeBuilder(IGamemodeBuilder builder)
        {
            this.builders = this.builders.Add(builder);
            this.UpdateServerAndGamemodeBuilders();

            return this;
        }

        /// <inheritdoc />
        public virtual IStartupDirector SetServerBuilder(IServerBuilder builder)
        {
            this.serverBuilder = builder;
            this.UpdateServerAndGamemodeBuilders();

            return this;
        }

        /// <inheritdoc />
        public virtual IStartupDirector Build()
        {
            Console.WriteLine("[SAMP.Net] Creating service collection...");

            var collection = this.serverBuilder.CreateServiceCollection();

            Console.WriteLine("[SAMP.Net] Following gamemode-builders have been registered:");

            foreach (var builder in this.serverAndGamemodeBuilders)
            {
                Console.WriteLine($"- {builder.GetType().FullName}");
            }

            Console.WriteLine("[SAMP.Net] Registering services...");
            foreach (var builder in this.serverAndGamemodeBuilders)
            {
                builder.RegisterServices(collection);
            }

            Console.WriteLine("[SAMP.Net] Setting up authorization...");
            collection.AddAuthorizationCore(options =>
                                            {
                                                foreach (var builder in this.serverAndGamemodeBuilders)
                                                {
                                                    builder.ConfigureAuthorization(options);
                                                }
                                            });

            Console.WriteLine("[SAMP.Net] Build service provider...");
            this.serviceProvider = this.serverBuilder.BuildServiceProvider(collection);

            return this;
        }

        /// <inheritdoc />
        public virtual void Start()
        {
            if (this.serviceProvider == null)
            {
                throw new InvalidOperationException(
                                                    $"Director not ready to start. Run {nameof(this.Build)} before this.");
            }

            this.serverBuilder.Start(this.serviceProvider);

            foreach (var builder in this.serverAndGamemodeBuilders)
            {
                builder.Start(this.serviceProvider);
            }
        }

        private void UpdateServerAndGamemodeBuilders()
        {
            this.serverAndGamemodeBuilders = this.builders.Prepend(this.serverBuilder).ToImmutableList();
        }
    }
}
