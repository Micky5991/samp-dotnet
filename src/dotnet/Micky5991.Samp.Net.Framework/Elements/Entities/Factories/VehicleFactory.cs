using System;
using System.Numerics;
using Micky5991.Samp.Net.Core.Natives.Samp;
using Micky5991.Samp.Net.Core.Natives.Vehicles;
using Micky5991.Samp.Net.Framework.Exceptions;
using Micky5991.Samp.Net.Framework.Interfaces.Entities;
using Micky5991.Samp.Net.Framework.Interfaces.Entities.Factories;
using Micky5991.Samp.Net.Framework.Interfaces.Entities.Pools;

namespace Micky5991.Samp.Net.Framework.Elements.Entities.Factories
{
    /// <inheritdoc />
    public class VehicleFactory : IVehicleFactory
    {
        private readonly ISampNatives sampNatives;

        private readonly IVehiclesNatives vehiclesNatives;

        /// <summary>
        /// Initializes a new instance of the <see cref="VehicleFactory"/> class.
        /// </summary>
        /// <param name="sampNatives">Natives which are needed for this factory.</param>
        /// <param name="vehiclesNatives">Natives to modify vehicle.</param>
        public VehicleFactory(ISampNatives sampNatives, IVehiclesNatives vehiclesNatives)
        {
            this.sampNatives = sampNatives;
            this.vehiclesNatives = vehiclesNatives;
        }

        /// <inheritdoc />
        public IVehicle CreateVehicle(
            Core.Natives.Samp.Vehicle model,
            Vector3 position,
            float rotation,
            int color1,
            int color2,
            bool addSiren,
            IEntityPool<IVehicle>.RemoveEntityDelegate entityRemoval)
        {
            var vehicleId = this.sampNatives.AddStaticVehicleEx(
                                                                (int)model,
                                                                position.X,
                                                                position.Y,
                                                                position.Z,
                                                                rotation,
                                                                color1,
                                                                color2,
                                                                -1,
                                                                addSiren);

            if (vehicleId == SampConstants.InvalidVehicleId)
            {
                throw new EntityLimitReachedException(typeof(IVehicle));
            }

            return new Vehicle(vehicleId, entityRemoval, this.vehiclesNatives);
        }

        /// <inheritdoc />
        public IVehicle CreateVehicle(
            Core.Natives.Samp.Vehicle model,
            Vector3 position,
            float rotation,
            int color1,
            int color2,
            TimeSpan respawnDelay,
            bool addSiren,
            IEntityPool<IVehicle>.RemoveEntityDelegate entityRemoval)
        {
            var vehicleId = this.sampNatives.AddStaticVehicleEx(
                                                                (int)model,
                                                                position.X,
                                                                position.Y,
                                                                position.Z,
                                                                rotation,
                                                                color1,
                                                                color2,
                                                                (int)respawnDelay.TotalSeconds,
                                                                addSiren);

            if (vehicleId == SampConstants.InvalidVehicleId)
            {
                throw new EntityLimitReachedException(typeof(IVehicle));
            }

            return new Vehicle(vehicleId, entityRemoval, this.vehiclesNatives);
        }
    }
}
