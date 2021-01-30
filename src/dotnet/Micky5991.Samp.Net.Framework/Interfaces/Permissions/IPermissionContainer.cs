using System;
using System.Collections.Immutable;
using Micky5991.Samp.Net.Framework.Enums.Permissions;

namespace Micky5991.Samp.Net.Framework.Interfaces.Permissions
{
    /// <summary>
    /// Container that holds various permissions. This can be a role or any other entity which provides a set of permission attachments.
    /// </summary>
    public interface IPermissionContainer
    {
        /// <summary>
        /// Event that will be triggered when the calulation of the permissions changed.
        /// </summary>
        event EventHandler<EventArgs> PermissionsUpdated;

        /// <summary>
        /// Adds a child <see cref="IPermissionContainer"/> instance with a specific tier.
        /// </summary>
        /// <param name="container">Container to add.</param>
        /// <param name="tier">Tier group of this container, higher value means higher priority.</param>
        void AttachChildPermissionContainer(IPermissionContainer container, int tier = 1);

        /// <summary>
        /// Adds a certain permission to the container. Will be merged with every <see cref="IPermissionContainer"/> of the same tier.
        /// </summary>
        /// <param name="permission">Permission that will be set.</param>
        /// <param name="value">Overwritten value of this <paramref name="permission"/>.</param>
        /// <param name="flags">Flags that specifies the handling and interpretation of this attachment.</param>
        /// <param name="tier">Tier group of this container, higher value means higher priority.</param>
        /// <param name="neededContexts">Context in which this permission applies.</param>
        /// <returns>Created permission attachment.</returns>
        IPermissionAttachment AttachPermission(
            IPermission permission,
            bool value,
            PermissionAttachmentFlag flags = PermissionAttachmentFlag.None,
            int tier = 1,
            IImmutableDictionary<string, string[]>? neededContexts = null);

        /// <summary>
        /// Compiles the effective set permissions for a certain context. Will exire as soon if the event
        /// <see cref="PermissionsUpdated"/> has been triggered.
        /// </summary>
        /// <param name="context">Context to base the permissions on.</param>
        /// <returns>Calculated permissions.</returns>
        IImmutableDictionary<IPermission, bool> GetEffectivePermissions(IImmutableDictionary<string, string> context);

        /// <summary>
        /// Gets the state of the permission. If the permission has not be defined.
        /// </summary>
        /// <param name="permission">Permission to get the state for.</param>
        /// <param name="context">Context to calculate the permissions based on.</param>
        /// <returns>true if this container would give the permission, false if permission is not defined or permission is not granted.</returns>
        bool HasPermission(string permission, IImmutableDictionary<string, string> context);

        /// <summary>
        /// Checks if the given <paramref name="permission"/> is defined.
        /// </summary>
        /// <param name="permission">Permission to check the existance.</param>
        /// <param name="context">Context to calculate the permission existance based on.</param>
        /// <returns>true if the permission has been defined, false otherwise.</returns>
        bool IsPermissionSet(string permission, IImmutableDictionary<string, string> context);
    }
}
