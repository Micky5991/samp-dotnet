using System.Collections.Immutable;
using Micky5991.Samp.Net.Framework.Enums.Permissions;

namespace Micky5991.Samp.Net.Framework.Interfaces.Permissions
{
    /// <summary>
    /// Describes an object that can hold permissions.
    /// </summary>
    public interface IPermissible
    {
        /// <summary>
        /// Checks if the current <see cref="IPermissible"/> has the permission to do something. If the permission was not set, <paramref name="defaultValue"/> will be returned.
        /// </summary>
        /// <param name="permission">Permission to check if the <see cref="IPermissible"/> is granted to do.</param>
        /// <param name="defaultValue">If the given <paramref name="permission"/> is not set, this value will be returned.</param>
        /// <returns>true if the <see cref="IPermissible"/> is granted to do something.</returns>
        bool HasPermission(string permission, bool defaultValue = false);

        /// <summary>
        /// Checks if the given <paramref name="permission"/> is set at oll on the current <see cref="IPermissible"/>.
        /// </summary>
        /// <param name="permission">Permission to check.</param>
        /// <returns>true if the permission is defined, false otherwise.</returns>
        bool IsPermissionSet(string permission);

        /// <summary>
        /// Attaches the given <paramref name="childPermissionContainer"/> to the container in this entity.
        /// </summary>
        /// <param name="childPermissionContainer">Container to add at tier.</param>
        /// <param name="tier">Tier group of this container, higher value means higher priority.</param>
        void AttachChildPermissionContainer(IPermissionContainer childPermissionContainer, int tier = 1);

        /// <summary>
        /// Removes the given permission container from the internal container of this entity.
        /// </summary>
        /// <param name="childPermissionContainer">Container to remove from this entity.</param>
        /// <param name="tier">Tier group where this container has been added to.</param>
        void RemovePermissionContainer(IPermissionContainer childPermissionContainer, int tier = 1);

        /// <summary>
        /// Attaches a permission to this entity.
        /// </summary>
        /// <param name="permission">Permission to add to this entity.</param>
        /// <param name="value">Value indicating if the permission will be granted.</param>
        /// <param name="flags">Flags that describe how this permission should be interpreted.</param>
        /// <param name="tier">Tier group of this container, higher value means higher priority.</param>
        /// <param name="neededContexts">Context in which this permission attachment applies. </param>
        /// <returns>The created attachment for this permission.</returns>
        IPermissionAttachment AttachPermission(
            IPermission permission,
            bool value,
            PermissionAttachmentFlag flags = PermissionAttachmentFlag.None,
            int tier = 1,
            IImmutableDictionary<string, string[]>? neededContexts = null);
    }
}
