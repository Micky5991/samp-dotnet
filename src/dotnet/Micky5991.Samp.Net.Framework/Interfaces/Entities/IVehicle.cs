using System;
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
        /// <exception cref="ObjectDisposedException"><see cref="IPlayer"/> is disposed.</exception>
        Vehicle Model { get; }

        /// <summary>
        /// Gets the quaternion rotation of this vehicle.
        /// </summary>
        /// <exception cref="ObjectDisposedException"><see cref="IPlayer"/> is disposed.</exception>
        Quaternion Quaternion { get; }

        /// <summary>
        /// Gets a value indicating whether the siren is on.
        /// </summary>
        /// <exception cref="ObjectDisposedException"><see cref="IPlayer"/> is disposed.</exception>
        bool SirenState { get; }

        /// <summary>
        /// Repairs the current vehicle.
        /// </summary>
        /// <exception cref="ObjectDisposedException"><see cref="IPlayer"/> is disposed.</exception>
        void Repair();

        /// <summary>
        /// Destroys the current vehicle. Alias for Dispose().
        /// </summary>
        /// <exception cref="ObjectDisposedException"><see cref="IPlayer"/> is disposed.</exception>
        void Destroy();

        /// <summary>
        /// Moves the vehicle back to spawn location.
        /// </summary>
        /// <exception cref="ObjectDisposedException"><see cref="IPlayer"/> is disposed.</exception>
        void SetToRespawn();

        /// <summary>
        /// Links this vehicle to the interior <paramref name="interiorId"/>.
        /// </summary>
        /// <param name="interiorId">New interior to set to.</param>
        /// <exception cref="ObjectDisposedException"><see cref="IPlayer"/> is disposed.</exception>
        void LinkToInterior(int interiorId);

        /// <summary>
        /// Changes the numberplate to a new value.
        /// </summary>
        /// <param name="numberplate">New numberplate content.</param>
        /// <exception cref="ObjectDisposedException"><see cref="IPlayer"/> is disposed.</exception>
        void SetNumberPlate(string numberplate);

        /// <summary>
        /// Sets the angular velocity of this vehicle.
        /// </summary>
        /// <param name="angularVelocity">New angular velocity to set.</param>
        /// <exception cref="ObjectDisposedException"><see cref="IPlayer"/> is disposed.</exception>
        void SetAngularVelocity(Vector3 angularVelocity);

        /// <summary>
        /// Indicates if the vehicle is streamed to the specificed player.
        /// </summary>
        /// <param name="forPlayer">Target player to check for.</param>
        /// <returns>true if streamed in, false otherwise.</returns>
        /// <exception cref="ObjectDisposedException"><see cref="IPlayer"/> is disposed.</exception>
        bool IsVehicleStreamedIn(IPlayer forPlayer);

        /// <summary>
        /// Updates the parameters of the vehicle.
        /// </summary>
        /// <param name="engine">true if the engine is running.</param>
        /// <param name="lights">true if the lights are on.</param>
        /// <param name="alarm">true if the alarm is (or was) sounding.</param>
        /// <param name="doors">true if the doors are open.</param>
        /// <param name="bonnet">true if the bonnet is open.</param>
        /// <param name="boot">true if the boot is open.</param>
        /// <param name="objective">true if the objective is visible.</param>
        /// <exception cref="ObjectDisposedException"><see cref="IPlayer"/> is disposed.</exception>
        void SetParamsEx(bool engine, bool lights, bool alarm, bool doors, bool bonnet, bool boot, bool objective);

        /// <summary>
        /// Gets the parameters of the vehicle.
        /// </summary>
        /// <param name="engine">true if the engine is running.</param>
        /// <param name="lights">true if the lights are on.</param>
        /// <param name="alarm">true if the alarm is (or was) sounding.</param>
        /// <param name="doors">true if the doors are open.</param>
        /// <param name="bonnet">true if the bonnet is open.</param>
        /// <param name="boot">true if the boot is open.</param>
        /// <param name="objective">true if the objective is visible.</param>
        /// <exception cref="ObjectDisposedException"><see cref="IPlayer"/> is disposed.</exception>
        void GetParamsEx(
            out bool engine,
            out bool lights,
            out bool alarm,
            out bool doors,
            out bool bonnet,
            out bool boot,
            out bool objective);

        /// <summary>
        /// Sets the car parameters for their car doors.
        /// </summary>
        /// <param name="driver">true if drivers door is open.</param>
        /// <param name="passenger">true if the passengers door is open.</param>
        /// <param name="backleft">true if the backleft door (if available) is open.</param>
        /// <param name="backright">true if the backright door (if available) is open.</param>
        /// <exception cref="ObjectDisposedException"><see cref="IPlayer"/> is disposed.</exception>
        void SetParamsCarDoors(bool driver, bool passenger, bool backleft, bool backright);

        /// <summary>
        /// Gets the car parameters for their car doors.
        /// </summary>
        /// <param name="driver">true if drivers door is open.</param>
        /// <param name="passenger">true if the passengers door is open.</param>
        /// <param name="backleft">true if the backleft door (if available) is open.</param>
        /// <param name="backright">true if the backright door (if available) is open.</param>
        /// <exception cref="ObjectDisposedException"><see cref="IPlayer"/> is disposed.</exception>
        void GetParamsCarDoors(out bool driver, out bool passenger, out bool backleft, out bool backright);

        /// <summary>
        /// Sets the parameters of their car windows.
        /// </summary>
        /// <param name="driver">true if drivers window is open.</param>
        /// <param name="passenger">true if the passengers window is open.</param>
        /// <param name="backleft">true if the backleft window (if available) is open.</param>
        /// <param name="backright">true if the backright window (if available) is open.</param>
        /// <exception cref="ObjectDisposedException"><see cref="IPlayer"/> is disposed.</exception>
        void SetParamsCarWindows(bool driver, bool passenger, bool backleft, bool backright);

        /// <summary>
        /// Gets the parameters of their car windows.
        /// </summary>
        /// <param name="driver">true if drivers window is open.</param>
        /// <param name="passenger">true if the passengers window is open.</param>
        /// <param name="backleft">true if the backleft window (if available) is open.</param>
        /// <param name="backright">true if the backright window (if available) is open.</param>
        /// <exception cref="ObjectDisposedException"><see cref="IPlayer"/> is disposed.</exception>
        void GetParamsCarWindows(out bool driver, out bool passenger, out bool backleft, out bool backright);
    }
}
