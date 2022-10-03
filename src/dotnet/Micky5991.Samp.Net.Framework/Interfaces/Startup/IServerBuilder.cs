// <copyright file="IServerBuilder.cs" company="Micky5991">
// Copyright (c) Micky5991. All rights reserved.
// </copyright>

using System;
using Microsoft.Extensions.DependencyInjection;

namespace Micky5991.Samp.Net.Framework.Interfaces.Startup
{
    /// <summary>
    /// Builds the server with its gamemode and child extensions.
    /// </summary>
    public interface IServerBuilder : IGamemodeBuilder
    {
        /// <summary>
        /// Creates the wrapping service collection instance in which <see cref="IGamemodeBuilder"/> can register services into.
        /// </summary>
        /// <returns>New <see cref="IServiceCollection"/> instance.</returns>
        IServiceCollection CreateServiceCollection();

        /// <summary>
        /// Takes the given <paramref name="serviceCollection"/> and turns it into a <see cref="IServiceProvider"/> instance.
        /// </summary>
        /// <param name="serviceCollection">List of services to use.</param>
        /// <returns>New <see cref="IServiceProvider"/> instance.</returns>
        IServiceProvider BuildServiceProvider(IServiceCollection serviceCollection);
    }
}
