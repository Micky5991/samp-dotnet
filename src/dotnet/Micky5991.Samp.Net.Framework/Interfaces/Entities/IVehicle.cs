using Micky5991.Samp.Net.Core.Natives.Samp;

namespace Micky5991.Samp.Net.Framework.Interfaces.Entities
{
    /// <summary>
    /// Represents a vehicle in the GTA world that can be used.
    /// </summary>
    public interface IVehicle : IWorldEntity
    {
        /// <summary>
        /// Gets the model of this vehicle.
        /// </summary>
        Vehicle Model { get; }
    }
}
