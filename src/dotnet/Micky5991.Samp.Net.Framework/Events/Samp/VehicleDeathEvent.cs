using System;
using Dawn;
using Micky5991.EventAggregator.Elements;
using Micky5991.Samp.Net.Framework.Interfaces.Entities;

namespace Micky5991.Samp.Net.Framework.Events.Samp
{
    /// <summary>
    /// Event that will be triggered when a vehicle dies.
    /// </summary>
    public class VehicleDeathEvent : EventBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VehicleDeathEvent"/> class.
        /// </summary>
        /// <param name="vehicle">Vehicle that died.</param>
        /// <param name="deathSyncer">Player that reported the vehicle death to the server.</param>
        /// <exception cref="ArgumentNullException"><paramref name="vehicle"/> or <paramref name="deathSyncer"/> is null.</exception>
        /// <exception cref="ObjectDisposedException"><paramref name="vehicle"/> or <paramref name="deathSyncer"/> was disposed.</exception>
        public VehicleDeathEvent(IVehicle vehicle, IPlayer deathSyncer)
        {
            Guard.Argument(vehicle, nameof(vehicle)).NotNull();
            Guard.Argument(deathSyncer, nameof(deathSyncer)).NotNull();

            Guard.Disposal(vehicle.Disposed, nameof(vehicle));
            Guard.Disposal(deathSyncer.Disposed, nameof(deathSyncer));

            this.Vehicle = vehicle;
            this.DeathSyncer = deathSyncer;
        }

        /// <summary>
        /// Gets the vehicle that died.
        /// </summary>
        public IVehicle Vehicle { get; }

        /// <summary>
        /// Gets the syncher of the vehicle destruction. Refered in the wiki as "killer", but its not always the killer.
        /// </summary>
        public IPlayer DeathSyncer { get; }
    }
}
