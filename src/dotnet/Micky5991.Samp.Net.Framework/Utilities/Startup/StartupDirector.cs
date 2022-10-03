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
        private IServerBuilder _serverBuilder;

        private IImmutableList<IGamemodeBuilder> builders = Array.Empty<IGamemodeBuilder>().ToImmutableList();

        /// <summary>
        /// Initializes a new instance of the <see cref="StartupDirector"/> class.
        /// </summary>
        /// <param name="serverBuilder">Initial <see cref="IServerBuilder"/> instance.</param>
        public StartupDirector(IServerBuilder serverBuilder)
        {
            this._serverBuilder = serverBuilder;
        }

        /// <inheritdoc />
        public virtual IStartupDirector AddGamemodeBuilder(IGamemodeBuilder builder)
        {
            this.builders = builders.Add(builder);

            return this;
        }

        /// <inheritdoc />
        public virtual IStartupDirector SetServerBuilder(IServerBuilder builder)
        {
            this._serverBuilder = builder;

            return this;
        }

        /// <inheritdoc />
        public virtual void Build()
        {
            var collection = this._serverBuilder.CreateServiceCollection();

            foreach (var builder in this.builders.Prepend(this._serverBuilder))
            {
                builder.RegisterServices(collection);
            }

            collection.AddAuthorizationCore(this._serverBuilder.ConfigureAuthorization);

            var provider = this._serverBuilder.BuildServiceProvider(collection);
        }

        /// <inheritdoc />
        public virtual void Start()
        {
            throw new NotImplementedException();
        }
    }
}
