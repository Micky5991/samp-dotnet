using Micky5991.EventAggregator.Interfaces;
using Micky5991.Samp.Net.Core.Natives.Samp;
using Micky5991.Samp.Net.Framework.Events.Players;
using Micky5991.Samp.Net.Framework.Interfaces.Entities;
using Micky5991.Samp.Net.Framework.Interfaces.Pools;
using Microsoft.Extensions.Logging;

namespace Micky5991.Samp.Net.Framework.Entities.Listeners
{
    /// <summary>
    /// Listens for specific <see cref="IVehicle"/> events and converts them to better event parameters.
    /// </summary>
    public class VehicleEventListener : EventListenerBase
    {
        private readonly ILogger<VehicleEventListener> logger;

        private readonly IVehiclePool vehiclePool;

        private readonly IPlayerPool playerPool;

        private readonly IEventAggregator eventAggregator;

        /// <summary>
        /// Initializes a new instance of the <see cref="VehicleEventListener"/> class.
        /// </summary>
        /// <param name="logger">Logger of this listener.</param>
        /// <param name="vehiclePool">Pool instance that will be used.</param>
        /// <param name="playerPool">Pool instance for players to resove players.</param>
        /// <param name="eventAggregator">Eventaggregator to attach the events to.</param>
        public VehicleEventListener(ILogger<VehicleEventListener> logger, IVehiclePool vehiclePool, IPlayerPool playerPool, IEventAggregator eventAggregator)
            : base(eventAggregator)
        {
            this.logger = logger;
            this.vehiclePool = vehiclePool;
            this.playerPool = playerPool;
            this.eventAggregator = eventAggregator;
        }

        /// <inheritdoc />
        public override void Attach()
        {
            this.eventAggregator.Subscribe<NativeVehicleDeathEvent>(this.OnVehicleDeath);
            this.eventAggregator.Subscribe<NativeVehicleSpawnEvent>(this.OnVehicleSpawn);
        }

        private void OnVehicleSpawn(NativeVehicleSpawnEvent eventdata)
        {
            if (this.vehiclePool.Entities.TryGetValue(eventdata.Vehicleid, out var vehicle) == false)
            {
                this.logger.LogWarning($"Received a {nameof(NativeVehicleSpawnEvent)} from vehicle {eventdata.Vehicleid}, but the vehicle could not be found.");

                return;
            }

            this.eventAggregator.Publish(new VehicleRespawnEvent(vehicle));
        }

        private void OnVehicleDeath(NativeVehicleDeathEvent eventdata)
        {
            if (this.vehiclePool.Entities.TryGetValue(eventdata.Vehicleid, out var vehicle) == false)
            {
                this.logger.LogWarning($"Received a {nameof(NativeVehicleDeathEvent)} from vehicle {eventdata.Vehicleid}, but the vehicle could not be found.");

                return;
            }

            if (this.playerPool.Entities.TryGetValue(eventdata.Killerid, out var deathSyncer) == false)
            {
                this.logger.LogWarning($"Received a {nameof(NativeVehicleDeathEvent)} from player {eventdata.Killerid}, but the player could not be found.");

                return;
            }

            this.eventAggregator.Publish(new VehicleDeathEvent(vehicle, deathSyncer));
        }
    }
}
