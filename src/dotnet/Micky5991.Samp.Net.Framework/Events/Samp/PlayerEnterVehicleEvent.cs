using System;
using Dawn;
using Micky5991.EventAggregator.Elements;
using Micky5991.Samp.Net.Framework.Interfaces.Entities;

namespace Micky5991.Samp.Net.Framework.Events.Samp
{
    /// <summary>
    /// Event that will be triggered when the player enters the vehicle.
    /// </summary>
    public class PlayerEnterVehicleEvent : CancellableEventBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PlayerEnterVehicleEvent"/> class.
        /// </summary>
        /// <param name="player">Player that tried to enter the vehicle.</param>
        /// <param name="vehicle">Vehicle which the <paramref name="player"/> tried to enter.</param>
        /// <param name="isPassenger">true if the player was about to become the driver, false otherwise.</param>
        /// <exception cref="ArgumentNullException"><paramref name="player"/> or <paramref name="vehicle"/> is null.</exception>
        /// <exception cref="ObjectDisposedException"><paramref name="player"/> or <paramref name="vehicle"/> was disposed.</exception>
        public PlayerEnterVehicleEvent(IPlayer player, IVehicle vehicle, bool isPassenger)
        {
            Guard.Argument(player, nameof(player)).NotNull();
            Guard.Argument(vehicle, nameof(vehicle)).NotNull();

            Guard.Disposal(player.Disposed, nameof(player));
            Guard.Disposal(vehicle.Disposed, nameof(vehicle));

            this.Player = player;
            this.Vehicle = vehicle;
            this.IsPassenger = isPassenger;
        }

        /// <summary>
        /// Gets the player that tries to enter the vehicle.
        /// </summary>
        public IPlayer Player { get; }

        /// <summary>
        /// Gets the vehicle which the <see cref="Player"/> tried to enter.
        /// </summary>
        public IVehicle Vehicle { get; }

        /// <summary>
        /// Gets a value indicating whether the <see cref="Player"/> tries to enter the vehicle as a driver.
        /// </summary>
        public bool IsPassenger { get; }
    }
}
