using System;
using Micky5991.EventAggregator.Elements;
using Micky5991.Samp.Net.Framework.Interfaces.Entities;

namespace Micky5991.Samp.Net.Framework.Events.Samp
{
    /// <summary>
    /// Event that will be triggered when a vehicle respawns. Orginal: OnVehicleSpawn.
    /// </summary>
    public class VehicleRespawnEvent : EventBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VehicleRespawnEvent"/> class.
        /// </summary>
        /// <param name="vehicle">Vehicle that respawned.</param>
        /// <exception cref="ArgumentNullException"><paramref name="vehicle"/> is null.</exception>
        /// <exception cref="ObjectDisposedException"><paramref name="vehicle"/> was disposed.</exception>
        public VehicleRespawnEvent(IVehicle vehicle)
        {
            this.Vehicle = vehicle;
        }

        /// <summary>
        /// Gets the vehicle that respawned.
        /// </summary>
        public IVehicle Vehicle { get; }
    }
}
