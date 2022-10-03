// <copyright file="IGamemodeBuilder.cs" company="Micky5991">
// Copyright (c) Micky5991. All rights reserved.
// </copyright>

using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace Micky5991.Samp.Net.Framework.Interfaces.Startup
{
    /// <summary>
    /// Builder that creates and starts a gamemode and its extensions.
    /// </summary>
    public interface IGamemodeBuilder
    {
        /// <summary>
        /// Registers services to the DI-Container.
        /// </summary>
        /// <param name="serviceCollection">List of services to register new services against.</param>
        void RegisterServices(IServiceCollection serviceCollection);

        /// <summary>
        /// Extends the current ASP.NET Core Authorization for commands and other functions.
        /// </summary>
        /// <param name="options">Options instance to edit.</param>
        void ConfigureAuthorization(AuthorizationOptions options);

        /// <summary>
        /// Starts all provided <see cref="IGamemodeBuilder"/> instances and their preparable services.
        /// </summary>
        /// <param name="serviceProvider">Created <see cref="IServiceProvider"/> of this builder.</param>
        void Start(IServiceProvider serviceProvider);
    }
}
