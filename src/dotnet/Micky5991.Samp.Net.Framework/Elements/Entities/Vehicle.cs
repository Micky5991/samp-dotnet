using System.Numerics;
using Dawn;
using Micky5991.Samp.Net.Core.Natives.Vehicles;
using Micky5991.Samp.Net.Framework.Interfaces.Entities;
using Micky5991.Samp.Net.Framework.Interfaces.Pools;

namespace Micky5991.Samp.Net.Framework.Elements.Entities
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
        public Quaternion Quaternion
        {
            get
            {
                Guard.Disposal(this.Disposed);

                this.vehiclesNatives.GetVehicleRotationQuat(this.Id, out var w, out var x, out var y, out var z);

                return new Quaternion(x, y, z, w);
            }
        }

        /// <inheritdoc />
        public bool SirenState
        {
            get
            {
                Guard.Disposal(this.Disposed);

                return this.vehiclesNatives.GetVehicleParamsSirenState(this.Id) == 1;
            }
        }

        /// <inheritdoc />
        public Vector3 Velocity
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
        public int VirtualWorld
        {
            get
            {
                Guard.Disposal(this.Disposed);

                return this.vehiclesNatives.GetVehicleVirtualWorld(this.Id);
            }

            set
            {
                Guard.Disposal(this.Disposed);

                this.vehiclesNatives.SetVehicleVirtualWorld(this.Id, value);
            }
        }

        /// <inheritdoc />
        public void Repair()
        {
            Guard.Disposal(this.Disposed);

            this.vehiclesNatives.RepairVehicle(this.Id);
        }

        /// <inheritdoc />
        public void Destroy()
        {
            this.Dispose();
        }

        /// <inheritdoc />
        public void SetToRespawn()
        {
            Guard.Disposal(this.Disposed);

            this.vehiclesNatives.SetVehicleToRespawn(this.Id);
        }

        /// <inheritdoc />
        public void LinkToInterior(int interiorId)
        {
            Guard.Disposal(this.Disposed);

            this.vehiclesNatives.LinkVehicleToInterior(this.Id, interiorId);
        }

        /// <inheritdoc />
        public void SetNumberPlate(string numberplate)
        {
            Guard.Disposal(this.Disposed);
            Guard.Argument(numberplate, nameof(numberplate)).NotNull();

            this.vehiclesNatives.SetVehicleNumberPlate(this.Id, numberplate);
        }

        /// <inheritdoc />
        public void SetAngularVelocity(Vector3 angularVelocity)
        {
            Guard.Disposal(this.Disposed);

            this.vehiclesNatives.SetVehicleAngularVelocity(
                                                           this.Id,
                                                           angularVelocity.X,
                                                           angularVelocity.Y,
                                                           angularVelocity.Z);
        }

        /// <inheritdoc />
        public bool IsVehicleStreamedIn(IPlayer forPlayer)
        {
            Guard.Argument(forPlayer, nameof(forPlayer)).NotNull();

            Guard.Disposal(this.Disposed);
            Guard.Disposal(forPlayer.Disposed, nameof(forPlayer));

            return this.vehiclesNatives.IsVehicleStreamedIn(this.Id, forPlayer.Id);
        }

        /// <inheritdoc />
        public void SetParamsEx(bool engine, bool lights, bool alarm, bool doors, bool bonnet, bool boot, bool objective)
        {
            Guard.Disposal(this.Disposed);

            this.vehiclesNatives.SetVehicleParamsEx(
                                                    this.Id,
                                                    engine ? 1 : 0,
                                                    lights ? 1 : 0,
                                                    alarm ? 1 : 0,
                                                    doors ? 1 : 0,
                                                    bonnet ? 1 : 0,
                                                    bonnet ? 1 : 0,
                                                    objective ? 1 : 0);
        }

        /// <inheritdoc />
        public void GetParamsEx(
            out bool engine,
            out bool lights,
            out bool alarm,
            out bool doors,
            out bool bonnet,
            out bool boot,
            out bool objective)
        {
            Guard.Disposal(this.Disposed);

            this.vehiclesNatives.GetVehicleParamsEx(
                                                    this.Id,
                                                    out var oEngine,
                                                    out var oLights,
                                                    out var oAlarm,
                                                    out var oDoors,
                                                    out var oBonnet,
                                                    out var oBoot,
                                                    out var oObjective);

            engine = oEngine == 1;
            lights = oLights == 1;
            alarm = oAlarm == 1;
            doors = oDoors == 1;
            bonnet = oBonnet == 1;
            boot = oBoot == 1;
            objective = oObjective == 1;
        }

        /// <inheritdoc />
        public void SetParamsCarDoors(bool driver, bool passenger, bool backleft, bool backright)
        {
            Guard.Disposal(this.Disposed);

            this.vehiclesNatives.SetVehicleParamsCarDoors(
                                                          this.Id,
                                                          driver ? 1 : 0,
                                                          passenger ? 1 : 0,
                                                          backleft ? 1 : 0,
                                                          backright ? 1 : 0);
        }

        /// <inheritdoc />
        public void GetParamsCarDoors(out bool driver, out bool passenger, out bool backleft, out bool backright)
        {
            Guard.Disposal(this.Disposed);

            this.vehiclesNatives.GetVehicleParamsCarDoors(
                                                          this.Id,
                                                          out var oDriver,
                                                          out var oPassenger,
                                                          out var oBackleft,
                                                          out var oBackright);

            driver = oDriver == 1;
            passenger = oPassenger == 1;
            backleft = oBackleft == 1;
            backright = oBackright == 1;
        }

        /// <inheritdoc />
        public void SetParamsCarWindows(bool driver, bool passenger, bool backleft, bool backright)
        {
            Guard.Disposal(this.Disposed);

            this.vehiclesNatives.SetVehicleParamsCarWindows(
                                                            this.Id,
                                                            driver ? 1 : 0,
                                                            passenger ? 1 : 0,
                                                            backleft ? 1 : 0,
                                                            backright ? 1 : 0);
        }

        /// <inheritdoc />
        public void GetParamsCarWindows(out bool driver, out bool passenger, out bool backleft, out bool backright)
        {
            Guard.Disposal(this.Disposed);

            this.vehiclesNatives.GetVehicleParamsCarWindows(
                                                            this.Id,
                                                            out var oDriver,
                                                            out var oPassenger,
                                                            out var oBackleft,
                                                            out var oBackright);

            driver = oDriver == 1;
            passenger = oPassenger == 1;
            backleft = oBackleft == 1;
            backright = oBackright == 1;
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
