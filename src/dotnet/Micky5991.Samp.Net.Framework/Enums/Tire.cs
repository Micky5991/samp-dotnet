using System;

namespace Micky5991.Samp.Net.Framework.Enums
{
    /// <summary>
    /// Indicates location of a specific tire and their bit position.
    /// </summary>
    [Flags]
    public enum Tire
    {
        /// <summary>
        /// No Panel.
        /// </summary>
        None = 0,

        /// <summary>
        /// Rear right on 4 tire vehicle or rear tire on 2 tire vehicle.
        /// </summary>
        RearRight = 1,

        /// <summary>
        /// Front right on 4 tire vehicle or front tire on 2 tire vehicle.
        /// </summary>
        FrontRight = 2,

        /// <summary>
        /// Rear left on 4 tire vehicle. Does not exist on 2 tire vehicle.
        /// </summary>
        RearLeft = 3,

        /// <summary>
        /// Front left on 4 tire vehicle. Does not exist on 2 tire vehicle.
        /// </summary>
        FrontLeft = 4,
    }
}
