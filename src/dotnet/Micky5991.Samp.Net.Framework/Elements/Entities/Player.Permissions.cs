using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace Micky5991.Samp.Net.Framework.Elements.Entities
{
    /// <inheritdoc cref="Micky5991.Samp.Net.Framework.Interfaces.Entities.IPlayer"/>
    public partial class Player
    {
        /// <inheritdoc />
        public bool HasPermission(string permission, bool defaultValue)
        {
            return this.permissionContainer.HasPermission(permission, this.GetPermissionContext());
        }

        /// <inheritdoc />
        public bool IsPermissionSet(string permission)
        {
            return this.permissionContainer.IsPermissionSet(permission, this.GetPermissionContext());
        }

        private IImmutableDictionary<string, string> GetPermissionContext()
        {
            return new Dictionary<string, string>().ToImmutableDictionary();
        }
    }
}
