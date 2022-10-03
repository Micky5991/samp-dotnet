// <copyright file="GamemodeBuilder.cs" company="Micky5991">
// Copyright (c) Micky5991. All rights reserved.
// </copyright>

using System;
using Micky5991.Samp.Net.Framework.Interfaces.Startup;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace Micky5991.Samp.Net.Framework.Utilities.Startup
{
    /// <inheritdoc />
    public abstract class GamemodeBuilder : IGamemodeBuilder
    {
        /// <inheritdoc />
        public virtual void RegisterServices(IServiceCollection serviceCollection)
        {
            // empty
        }

        /// <inheritdoc />
        public virtual void ConfigureAuthorization(AuthorizationOptions options)
        {
            // empty
        }

        /// <inheritdoc />
        public virtual void Start(IServiceProvider serviceProvider)
        {
            // empty
        }
    }
}
