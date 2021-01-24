using System;

namespace Micky5991.Samp.Net.Framework.Enums
{
    /// <summary>
    /// List of all available states of a specific door. Describes the zero-based bit position in the door-byte.
    /// </summary>
    [Flags]
    public enum DoorState
    {
        /// <summary>
        /// No specific door state.
        /// </summary>
        None = -1,

        /// <summary>
        /// Door is opened.
        /// </summary>
        Opened = 0,

        /// <summary>
        /// Door is damaged.
        /// </summary>
        Damaged = 1,

        /// <summary>
        /// Door is removed.
        /// </summary>
        Removed = 2,
    }
}
