using System;
using System.Numerics;
using Micky5991.Samp.Net.Core.Natives.Samp;
using Micky5991.Samp.Net.Framework.Exceptions;
using Micky5991.Samp.Net.Framework.Interfaces.Entities;
using Micky5991.Samp.Net.Framework.Interfaces.Pools;

namespace Micky5991.Samp.Net.Framework.Interfaces.Factories
{
    /// <summary>
    /// Factory to create vehicles.
    /// </summary>
    public interface IVehicleFactory
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
        /// <param name="entityRemoval">Delegate that should be called when the vehicle should be removed.</param>
        /// <returns>Created vehicle.</returns>
        /// <exception cref="EntityLimitReachedException">Vehicle could not be created, limit reached.</exception>
        IVehicle CreateVehicle(Vehicle model, Vector3 position, float rotation, int color1, int color2, bool addSiren, IVehiclePool.RemoveEntityDelegate entityRemoval);

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
        /// <param name="entityRemoval">Delegate that should be called when the vehicle should be removed.</param>
        /// <returns>Created vehicle.</returns>
        /// <exception cref="EntityLimitReachedException">Vehicle could not be created, limit reached.</exception>
        IVehicle CreateVehicle(Vehicle model, Vector3 position, float rotation, int color1, int color2, TimeSpan respawnDelay, bool addSiren, IVehiclePool.RemoveEntityDelegate entityRemoval);
    }
}
