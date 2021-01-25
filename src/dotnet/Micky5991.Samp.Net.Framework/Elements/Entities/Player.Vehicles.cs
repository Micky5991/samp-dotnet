using Dawn;
using Micky5991.Samp.Net.Core.Natives.Samp;
using Micky5991.Samp.Net.Framework.Interfaces.Entities;

namespace Micky5991.Samp.Net.Framework.Elements.Entities
{
    /// <inheritdoc cref="IPlayer" />
    public partial class Player
    {
        /// <inheritdoc />
        public int? VehicleId
        {
            get
            {
                Guard.Disposal(this.Disposed);

                var vehicleId = this.playersNatives.GetPlayerVehicleID(this.Id);

                if (vehicleId == SampConstants.InvalidVehicleId)
                {
                    return null;
                }

                return vehicleId;
            }
        }

        /// <inheritdoc />
        public int? VehicleSeat
        {
            get
            {
                Guard.Disposal(this.Disposed);

                var seat = this.playersNatives.GetPlayerVehicleSeat(this.Id);

                if (seat == -1)
                {
                    return null;
                }

                return seat;
            }
        }

        /// <inheritdoc />
        public bool PutPlayerIntoVehicle(IVehicle vehicle, int seat = 0)
        {
            Guard.Argument(vehicle, nameof(vehicle)).NotNull();
            Guard.Argument(seat, nameof(seat)).NotNegative();
            Guard.Disposal(this.Disposed);
            Guard.Disposal(vehicle.Disposed, nameof(vehicle));

            return this.playersNatives.PutPlayerInVehicle(this.Id, vehicle.Id, seat);
        }

        /// <inheritdoc />
        public int? GetSurfingVehicleId()
        {
            Guard.Disposal(this.Disposed);

            var vehicle = this.playersNatives.GetPlayerSurfingVehicleID(this.Id);

            if (vehicle == SampConstants.InvalidVehicleId)
            {
                return null;
            }

            return vehicle;
        }

        /// <inheritdoc />
        public void RemoveFromVehicle()
        {
            Guard.Disposal(this.Disposed);

            this.playersNatives.RemovePlayerFromVehicle(this.Id);
        }

        /// <inheritdoc />
        public void DisableRemoteVehicleCollisions(bool disable)
        {
            Guard.Disposal(this.Disposed);

            this.playersNatives.DisableRemoteVehicleCollisions(this.Id, disable);
        }
    }
}
