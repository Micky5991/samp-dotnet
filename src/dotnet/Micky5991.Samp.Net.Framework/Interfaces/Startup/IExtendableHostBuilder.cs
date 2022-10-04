// <copyright file="IGamemodeBuilder.cs" company="Micky5991">
// Copyright (c) Micky5991. All rights reserved.
// </copyright>

using System;
using Microsoft.Extensions.Hosting;

namespace Micky5991.Samp.Net.Framework.Interfaces.Startup
{
    /// <summary>
    /// Builder that creates and starts a gamemode and its extensions.
    /// </summary>
    public interface IExtendableHostBuilder
    {
        /// <summary>
        /// Configures the host and its aspects.
        /// </summary>
        /// <param name="hostBuilder"><see cref="IHostBuilder"/> instance to configure.</param>
        /// <returns>Original <see cref="IHostBuilder"/> instanced passed in <paramref name="hostBuilder"/>.</returns>
        void ConfigureHost(IHostBuilder hostBuilder);
    }
}
