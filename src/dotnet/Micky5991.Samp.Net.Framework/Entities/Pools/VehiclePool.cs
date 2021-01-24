using System;
using System.Numerics;
using Dawn;
using Micky5991.Samp.Net.Core.Natives.Vehicles;
using Micky5991.Samp.Net.Framework.Interfaces.Entities;
using Micky5991.Samp.Net.Framework.Interfaces.Factories;
using Micky5991.Samp.Net.Framework.Interfaces.Pools;

namespace Micky5991.Samp.Net.Framework.Entities.Pools
{
    /// <inheritdoc cref="IVehiclePool"/>
    public class VehiclePool : EntityPool<IVehicle>, IVehiclePool
    {
        private readonly IVehicleFactory vehicleFactory;

        private readonly IVehiclesNatives vehiclesNatives;

        /// <summary>
        /// Initializes a new instance of the <see cref="VehiclePool"/> class.
        /// </summary>
        /// <param name="vehicleFactory">Factory that creates vehicles.</param>
        /// <param name="vehiclesNatives">Natives needed for certain pool methods.</param>
        public VehiclePool(IVehicleFactory vehicleFactory, IVehiclesNatives vehiclesNatives)
        {
            this.vehicleFactory = vehicleFactory;
            this.vehiclesNatives = vehiclesNatives;
        }

        /// <inheritdoc />
        public IVehicle CreateVehicle(
            Micky5991.Samp.Net.Core.Natives.Samp.Vehicle model,
            Vector3 position,
            float rotation,
            int color1,
            int color2,
            bool addSiren)
        {
            Guard.Argument(model, nameof(model)).Defined();

            var vehicle = this.vehicleFactory.CreateVehicle(model, position, rotation, color1, color2, addSiren, this.RemoveEntity);

            this.AddEntity(vehicle);

            return vehicle;
        }

        /// <inheritdoc />
        public IVehicle CreateVehicle(
            Micky5991.Samp.Net.Core.Natives.Samp.Vehicle model,
            Vector3 position,
            float rotation,
            int color1,
            int color2,
            TimeSpan respawnDelay,
            bool addSiren)
        {
            Guard.Argument(model, nameof(model)).Defined();

            var vehicle = this.vehicleFactory.CreateVehicle(model, position, rotation, color1, color2, respawnDelay, addSiren, this.RemoveEntity);

            this.AddEntity(vehicle);

            return vehicle;
        }

        /// <inheritdoc />
        public void ManualVehicleEngineAndLights()
        {
            this.vehiclesNatives.ManualVehicleEngineAndLights();
        }

        /// <inheritdoc />
        public Vector3 GetVehicleModelInfo(Core.Natives.Samp.Vehicle model, VehicleModelInfo infoType)
        {
            this.vehiclesNatives.GetVehicleModelInfo((int)model, (int)infoType, out var x, out var y, out var z);

            return new Vector3(x, y, z);
        }
    }
}
