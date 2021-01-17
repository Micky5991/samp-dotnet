using System;
using System.Drawing;

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

        /// <summary>
        /// Gets or sets the money of this player.
        /// </summary>
        /// <exception cref="ObjectDisposedException"><see cref="IPlayer"/> is disposed.</exception>
        int Money { get; set; }

        /// <summary>
        /// Gets or sets the current nametag color of this player.
        /// </summary>
        /// <exception cref="ObjectDisposedException"><see cref="IPlayer"/> is disposed.</exception>
        Color Color { get; set; }

        /// <summary>
        /// Gets or sets the current health of this player.
        /// </summary>
        /// <exception cref="ObjectDisposedException"><see cref="IPlayer"/> is disposed.</exception>
        float Health { get; set; }

        /// <summary>
        /// Gets or sets the current armor of this player.
        /// </summary>
        /// <exception cref="ObjectDisposedException"><see cref="IPlayer"/> is disposed.</exception>
        float Armor { get; set; }

        /// <summary>
        /// Gets the current animation index of this player.
        /// </summary>
        /// <exception cref="ObjectDisposedException"><see cref="IPlayer"/> is disposed.</exception>
        int AnimationIndex { get; }

        /// <summary>
        /// Puts the current player into the specified vehicle.
        /// </summary>
        /// <param name="vehicle">Target vehicle to set the player in.</param>
        /// <param name="seat">Seat of the vehicle, 0 = driver.</param>
        /// <returns>true if the player was set into the vehicle, false otherwise.</returns>
        /// <exception cref="ObjectDisposedException"><paramref name="vehicle"/> or <see cref="IPlayer"/> was disposed.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="seat"/> is negative.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="vehicle"/> is null.</exception>
        bool PutPlayerIntoVehicle(IVehicle vehicle, int seat = 0);
    }
}
