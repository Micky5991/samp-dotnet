using System;
using System.Numerics;
using Dawn;
using Micky5991.Samp.Net.Core.Natives.Vehicles;
using Micky5991.Samp.Net.Framework.Interfaces.Entities;
using Micky5991.Samp.Net.Framework.Interfaces.Pools;

namespace Micky5991.Samp.Net.Framework.Entities
{
    /// <inheritdoc cref="IVehicle"/>
    public class Vehicle : Entity, IVehicle
    {
        private readonly IEntityPool<IVehicle>.RemoveEntityDelegate entityRemoval;

        private readonly IVehiclesNatives vehiclesNatives;

        /// <summary>
        /// Initializes a new instance of the <see cref="Vehicle"/> class.
        /// </summary>
        /// <param name="id">Id for this vehicle.</param>
        /// <param name="entityRemoval">Delegate that should be used to remove this entity from the <see cref="IVehiclePool"/>.</param>
        /// <param name="vehiclesNatives">Natives which are needed for this entity.</param>
        public Vehicle(int id, IVehiclePool.RemoveEntityDelegate entityRemoval, IVehiclesNatives vehiclesNatives)
            : base(id)
        {
            this.entityRemoval = entityRemoval;
            this.vehiclesNatives = vehiclesNatives;
        }

        /// <inheritdoc />
        public Vector3 Position
        {
            get
            {
                Guard.Disposal(this.Disposed);

                this.vehiclesNatives.GetVehiclePos(this.Id, out var x, out var y, out var z);

                return new Vector3(x, y, z);
            }

            set
            {
                Guard.Disposal(this.Disposed);

                this.vehiclesNatives.SetVehiclePos(this.Id, value.X, value.Y, value.Z);
            }
        }

        /// <inheritdoc />
        public float Rotation
        {
            get
            {
                Guard.Disposal(this.Disposed);

                this.vehiclesNatives.GetVehicleZAngle(this.Id, out var rotation);

                return rotation;
            }

            set
            {
                Guard.Disposal(this.Disposed);

                this.vehiclesNatives.SetVehicleZAngle(this.Id, value);
            }
        }

        /// <inheritdoc />
        public Core.Natives.Samp.Vehicle Model
        {
            get
            {
                Guard.Disposal(this.Disposed);

                return (Core.Natives.Samp.Vehicle)this.vehiclesNatives.GetVehicleModel(this.Id);
            }
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return $"<{this.GetType()} ({this.Id}) {this.Model}>";
        }

        /// <inheritdoc />
        protected override void DisposeEntity()
        {
            this.entityRemoval(this);

            this.vehiclesNatives.DestroyVehicle(this.Id);
        }
    }
}
