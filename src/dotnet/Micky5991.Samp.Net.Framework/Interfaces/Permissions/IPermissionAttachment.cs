using System.Collections.Immutable;
using Micky5991.Samp.Net.Framework.Enums.Permissions;

namespace Micky5991.Samp.Net.Framework.Interfaces.Permissions
{
    /// <summary>
    /// Definition of this specific permission relation between an <see cref="IPermissionContainer"/> and <see cref="IPermissible"/>.
    /// </summary>
    public interface IPermissionAttachment
    {
        /// <summary>
        /// Gets the permission this attachment describes.
        /// </summary>
        public IPermission Permission { get; }

        /// <summary>
        /// Gets or sets a value indicating whether the permission will be granted.
        /// </summary>
        public bool Value { get; set; }

        /// <summary>
        /// Gets the flags that describe how this attachment should be interpreted.
        /// </summary>
        public PermissionAttachmentFlag Flags { get; }

        /// <summary>
        /// Gets the optional contexts that the <see cref="IPermissible"/> instantce has to satisfy to grant.
        /// </summary>
        public IImmutableDictionary<string, string[]>? NeededContexts { get; }
    }
}
