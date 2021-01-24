using System.Numerics;
using Micky5991.Samp.Net.Core.Natives.Samp;

namespace Micky5991.Samp.Net.Framework.Interfaces.Entities
{
    /// <summary>
    /// Represents a vehicle in the GTA world that can be used.
    /// </summary>
    public interface IVehicle : IMovingWorldEntity
    {
        /// <summary>
        /// Gets the model of this vehicle.
        /// </summary>
        Vehicle Model { get; }

        /// <summary>
        /// Gets the quaternion rotation of this vehicle.
        /// </summary>
        Quaternion Quaternion { get; }

        /// <summary>
        /// Repairs the current vehicle.
        /// </summary>
        void Repair();

        /// <summary>
        /// Destroys the current vehicle. Alias for Dispose().
        /// </summary>
        void Destroy();

        /// <summary>
        /// Moves the vehicle back to spawn location.
        /// </summary>
        void SetToRespawn();

        /// <summary>
        /// Links this vehicle to the interior <paramref name="interiorId"/>.
        /// </summary>
        /// <param name="interiorId">New interior to set to.</param>
        void LinkToInterior(int interiorId);

        /// <summary>
        /// Changes the numberplate to a new value.
        /// </summary>
        /// <param name="numberplate">New numberplate content.</param>
        void SetNumberPlate(string numberplate);

        /// <summary>
        /// Sets the angular velocity of this vehicle.
        /// </summary>
        /// <param name="angularVelocity">New angular velocity to set.</param>
        void SetAngularVelocity(Vector3 angularVelocity);

        /// <summary>
        /// Indicates if the vehicle is streamed to the specificed player.
        /// </summary>
        /// <param name="forPlayer">Target player to check for.</param>
        /// <returns>true if streamed in, false otherwise.</returns>
        bool IsVehicleStreamedIn(IPlayer forPlayer);
    }
}
