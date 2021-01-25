using System;
using System.Numerics;
using Micky5991.Samp.Net.Core.Natives.Samp;
using Micky5991.Samp.Net.Core.Natives.Vehicles;
using Micky5991.Samp.Net.Framework.Exceptions;

namespace Micky5991.Samp.Net.Framework.Interfaces.Entities.Pools
{
    /// <summary>
    /// Container of <see cref="IVehicle"/> instances.
    /// </summary>
    public interface IVehiclePool : IEntityPool<IVehicle>
    {
        /// <summary>
        /// Creates a static vehicle and adds it to the pool. These vehicles do not respawn.
        /// </summary>
        /// <param name="model">Desired model for this vehicle.</param>
        /// <param name="position">Position of this vehicle.</param>
        /// <param name="rotation">Rotation of the vehicle.</param>
        /// <param name="color1">Primary color of this vehicle.</param>
        /// <param name="color2">Secondary color of this vehicle.</param>
        /// <param name="addSiren">true when the vehicle should have a siren.</param>
        /// <returns>Created vehicle.</returns>
        /// <exception cref="EntityLimitReachedException">Vehicle could not be created, limit reached.</exception>
        IVehicle CreateVehicle(Vehicle model, Vector3 position, float rotation, int color1, int color2, bool addSiren = false);

        /// <summary>
        /// Creates a static vehicle and adds it to the pool. These vehicles respawn after <paramref name="respawnDelay"/>.
        /// </summary>
        /// <param name="model">Desired model for this vehicle.</param>
        /// <param name="position">Position of this vehicle.</param>
        /// <param name="rotation">Rotation of the vehicle.</param>
        /// <param name="color1">Primary color of this vehicle.</param>
        /// <param name="color2">Secondary color of this vehicle.</param>
        /// <param name="respawnDelay">Time to respawn.</param>
        /// <param name="addSiren">true when the vehicle should have a siren.</param>
        /// <returns>Created vehicle.</returns>
        /// <exception cref="EntityLimitReachedException">Vehicle could not be created, limit reached.</exception>
        IVehicle CreateVehicle(Vehicle model, Vector3 position, float rotation, int color1, int color2, TimeSpan respawnDelay, bool addSiren = false);

        /// <summary>
        /// Diesables the vehicle engine and light automation.
        /// </summary>
        void ManualVehicleEngineAndLights();

        /// <summary>
        /// Returns dimensional information about a specific vehicle model.
        /// </summary>
        /// <param name="model">Vehicle model to check.</param>
        /// <param name="infoType">Type of information to return.</param>
        /// <returns>Resulting information about this vehicle.</returns>
        Vector3 GetVehicleModelInfo(Vehicle model, VehicleModelInfo infoType);
    }
}
