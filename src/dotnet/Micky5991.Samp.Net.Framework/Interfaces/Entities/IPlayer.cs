using System;

namespace Micky5991.Samp.Net.Framework.Interfaces.Entities
{
    /// <summary>
    /// Represents a player on the server.
    /// </summary>
    public interface IPlayer : IWorldEntity
    {
        /// <summary>
        /// Gets or sets the current name of the player.
        /// </summary>
        /// <exception cref="ObjectDisposedException"><see cref="IPlayer"/> is disposed.</exception>
        /// <exception cref="ArgumentException">Value is empty or invalid length.</exception>
        string Name { get; set; }
    }
}
