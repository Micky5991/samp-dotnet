using System.Collections.Generic;
using System.Collections.Immutable;
using Micky5991.Samp.Net.Framework.Enums.Permissions;
using Micky5991.Samp.Net.Framework.Interfaces.Permissions;

namespace Micky5991.Samp.Net.Framework.Elements.Entities
{
    /// <inheritdoc cref="Player"/>
    public partial class Player
    {
        /// <inheritdoc />
        public bool HasPermission(string permission, bool defaultValue)
        {
            return this.permissionContainer.HasPermission(permission, this.permissionFactory.CalculateContext(this));
        }

        /// <inheritdoc />
        public bool IsPermissionSet(string permission)
        {
            return this.permissionContainer.IsPermissionSet(permission, this.permissionFactory.CalculateContext(this));
        }

        /// <inheritdoc />
        public void AttachChildPermissionContainer(IPermissionContainer childPermissionContainer, int tier = 1)
        {
            this.permissionContainer.AttachChildPermissionContainer(childPermissionContainer, tier);
        }

        /// <inheritdoc />
        public void RemovePermissionContainer(IPermissionContainer childPermissionContainer, int tier = 1)
        {
            this.permissionContainer.RemovePermissionContainer(childPermissionContainer, tier);
        }

        /// <inheritdoc />
        public IPermissionAttachment AttachPermission(
            IPermission permission,
            bool value,
            PermissionAttachmentFlag flags = PermissionAttachmentFlag.None,
            int tier = 1,
            IImmutableDictionary<string, string[]>? neededContexts = null)
        {
            return this.permissionContainer.AttachPermission(permission, value, flags, tier, neededContexts);
        }
    }
}
