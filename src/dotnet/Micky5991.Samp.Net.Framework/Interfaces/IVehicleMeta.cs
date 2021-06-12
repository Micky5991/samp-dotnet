using Micky5991.Samp.Net.Core.Natives.Samp;

namespace Micky5991.Samp.Net.Framework.Interfaces
{
    /// <summary>
    /// Contains static data about the vehicles themself.
    /// </summary>
    public interface IVehicleMeta
    {
        /// <summary>
        /// Gets the display name of this vehicle.
        /// </summary>
        /// <param name="model">Model of the vehicle to return the name for.</param>
        /// <returns>Returns the name of the vehicle, null otherwise.</returns>
        public string? GetVehicleName(Vehicle model);
    }
}
