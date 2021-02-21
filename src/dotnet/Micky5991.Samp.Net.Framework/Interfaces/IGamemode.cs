using System;

namespace Micky5991.Samp.Net.Framework.Interfaces
{
    /// <summary>
    /// Host gamemode that is able to dispatch gamemode starting and main gamemode.
    /// </summary>
    public interface IGamemode
    {
        /// <summary>
        /// Gets the constructed service provider of this gamemode.
        /// </summary>
        IServiceProvider ServiceProvider { get; }

        /// <summary>
        /// Starts the gamemode.
        /// </summary>
        void Start();
    }
}
