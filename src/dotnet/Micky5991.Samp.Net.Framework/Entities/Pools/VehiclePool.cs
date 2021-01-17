using System;
using System.Numerics;
using Dawn;
using Micky5991.Samp.Net.Core.Natives.Samp;
using Micky5991.Samp.Net.Framework.Exceptions;
using Micky5991.Samp.Net.Framework.Interfaces.Entities;
using Micky5991.Samp.Net.Framework.Interfaces.Factories;
using Micky5991.Samp.Net.Framework.Interfaces.Pools;

namespace Micky5991.Samp.Net.Framework.Entities.Pools
{
    /// <inheritdoc cref="IVehiclePool"/>
    public class VehiclePool : EntityPool<IVehicle>, IVehiclePool
    {
        private readonly IVehicleFactory vehicleFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="VehiclePool"/> class.
        /// </summary>
        /// <param name="vehicleFactory">Factory that creates vehicles.</param>
        public VehiclePool(IVehicleFactory vehicleFactory)
        {
            this.vehicleFactory = vehicleFactory;
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
    }
}
