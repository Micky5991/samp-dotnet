using System;

namespace Micky5991.Samp.Net.Framework.Enums
{
    /// <summary>
    /// List of all door positions and their zero-based byte position in an integer.
    /// </summary>
    [Flags]
    public enum Door
    {
        /// <summary>
        /// No specific door.
        /// </summary>
        None = -1,

        /// <summary>
        /// Hood on a car.
        /// </summary>
        Hood = 0,

        /// <summary>
        /// Trunk on a car.
        /// </summary>
        Trunk = 1,

        /// <summary>
        /// Drivers door.
        /// </summary>
        Driver = 2,

        /// <summary>
        /// Co-Drivers door.
        /// </summary>
        CoDriver = 3,
    }
}
