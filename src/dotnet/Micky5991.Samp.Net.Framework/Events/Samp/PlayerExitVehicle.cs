using System;
using Dawn;
using Micky5991.EventAggregator.Elements;
using Micky5991.Samp.Net.Framework.Interfaces.Entities;

namespace Micky5991.Samp.Net.Framework.Events.Samp
{
    /// <summary>
    /// Event that will be triggered when a player exists.
    /// </summary>
    public class PlayerExitVehicle : EventBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PlayerExitVehicle"/> class.
        /// </summary>
        /// <param name="player">Player that exited the <paramref name="vehicle"/>.</param>
        /// <param name="vehicle">Vehicle that has been exited.</param>
        /// <exception cref="ArgumentNullException"><paramref name="player"/> or <paramref name="vehicle"/> is null.</exception>
        /// <exception cref="ObjectDisposedException"><paramref name="player"/> or <paramref name="vehicle"/> was disposed.</exception>
        public PlayerExitVehicle(IPlayer player, IVehicle vehicle)
        {
            Guard.Argument(player, nameof(player)).NotNull();
            Guard.Argument(vehicle, nameof(vehicle)).NotNull();

            Guard.Disposal(player.Disposed, nameof(player));
            Guard.Disposal(vehicle.Disposed, nameof(vehicle));

            this.Player = player;
            this.Vehicle = vehicle;
        }

        /// <summary>
        /// Gets the player that exited the <see cref="Vehicle"/>.
        /// </summary>
        public IPlayer Player { get; }

        /// <summary>
        /// Gets the vehicle that has been exited.
        /// </summary>
        public IVehicle Vehicle { get; }
    }
}
