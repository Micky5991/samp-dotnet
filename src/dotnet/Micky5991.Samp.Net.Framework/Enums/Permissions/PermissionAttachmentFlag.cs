using System;

namespace Micky5991.Samp.Net.Framework.Enums.Permissions
{
    /// <summary>
    /// Flags of this specific permission attachment.
    /// </summary>
    [Flags]
    public enum PermissionAttachmentFlag
    {
        /// <summary>
        /// No specific flag.
        /// </summary>
        None = 0b0,

        /// <summary>
        /// When this flag is set, all following tiers are not processed and the value of this current tier will be returned.
        /// </summary>
        Skip = 0b1,

        /// <summary>
        /// When this flag is set, the calculation of this permission will be flipped and instead of the highest value, the lowest will be used.
        /// </summary>
        Negate = 0b01,
    }
}
