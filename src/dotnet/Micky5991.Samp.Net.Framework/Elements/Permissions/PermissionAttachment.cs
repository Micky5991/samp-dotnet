using System;
using System.Collections.Immutable;
using Dawn;
using Micky5991.Samp.Net.Framework.Enums.Permissions;
using Micky5991.Samp.Net.Framework.Interfaces.Permissions;

namespace Micky5991.Samp.Net.Framework.Elements.Permissions
{
    /// <summary>
    /// Simple <see cref="IPermissionAttachment"/> implementation.
    /// </summary>
    public class PermissionAttachment : IPermissionAttachment
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PermissionAttachment"/> class.
        /// </summary>
        /// <param name="permission">Linked permission instance to this attachment.</param>
        /// <param name="value">Actual value related to this <paramref name="permission"/>.</param>
        /// <param name="flags">Flags that define the interpretation of this permission.</param>
        /// <param name="neededContexts">Context that should be satisfied for this permission to apply.</param>
        /// <exception cref="ArgumentNullException"><paramref name="permission"/> is null.</exception>
        public PermissionAttachment(IPermission permission, bool value, PermissionAttachmentFlag flags, IImmutableDictionary<string, string[]>? neededContexts)
        {
            Guard.Argument(permission, nameof(permission));

            this.Permission = permission;
            this.Value = value;
            this.Flags = flags;
            this.NeededContexts = neededContexts;
        }

        /// <inheritdoc />
        public IPermission Permission { get; }

        /// <inheritdoc />
        public bool Value { get; set; }

        /// <inheritdoc />
        public PermissionAttachmentFlag Flags { get; }

        /// <inheritdoc />
        public IImmutableDictionary<string, string[]>? NeededContexts { get; }
    }
}
