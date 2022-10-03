// <copyright file="IStartupDirector.cs" company="Micky5991">
// Copyright (c) Micky5991. All rights reserved.
// </copyright>

namespace Micky5991.Samp.Net.Framework.Interfaces.Startup
{
    /// <summary>
    /// Director that handles the preparation of the gamemode and starts it afterwards.
    /// </summary>
    public interface IStartupDirector
    {
        /// <summary>
        /// Adds the specified <see cref="IGamemodeBuilder"/> to the chain of gamemode builders.
        /// </summary>
        /// <param name="builder">Gamemode builder to attach to director.</param>
        /// <returns>Current <see cref="IStartupDirector"/> instance.</returns>
        IStartupDirector AddGamemodeBuilder(IGamemodeBuilder builder);

        /// <summary>
        /// Adds the main <see cref="IServerBuilder"/> to the director.
        /// </summary>
        /// <param name="serverBuilder">Main <see cref="IServerBuilder"/> to use for startup.</param>
        /// <returns>Current <see cref="IStartupDirector"/> instance.</returns>
        IStartupDirector SetServerBuilder(IServerBuilder serverBuilder);

        /// <summary>
        /// Prepares the gamemode before bootup like service registration and configuration parsing.
        /// </summary>
        void Build();

        /// <summary>
        /// Starts the gamemode and its services.
        /// </summary>
        void Start();
    }
}
