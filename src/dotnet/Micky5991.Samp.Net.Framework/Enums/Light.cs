using System;

namespace Micky5991.Samp.Net.Framework.Enums
{
    /// <summary>
    /// List of all specific light positions and their bit position.
    /// </summary>
    [Flags]
    public enum Light
    {
        /// <summary>
        /// No specific location.
        /// </summary>
        None = -1,

        /// <summary>
        /// Front left light.
        /// </summary>
        FrontLeft = 0,

        /// <summary>
        /// Front right light.
        /// </summary>
        FrontRight = 2,

        /// <summary>
        /// Back light.
        /// </summary>
        Back = 6,
    }
}
