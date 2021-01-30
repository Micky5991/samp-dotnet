using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using Micky5991.Samp.Net.Framework.Enums.Permissions;
using Micky5991.Samp.Net.Framework.Interfaces.Entities;
using Micky5991.Samp.Net.Framework.Interfaces.Permissions;
using Microsoft.Extensions.Logging;

namespace Micky5991.Samp.Net.Framework.Extensions.FrameworkExtensions.Permissions.RconPermissions.Services
{
    /// <summary>
    /// Adds a very simple permission container that only grants <see cref="IPlayer"/> permissions if the player is an admin.
    /// </summary>
    public class RconPermissionContainer : IPermissionContainer
    {
        private readonly IPermissible permissible;

        private readonly ILogger<RconPermissionContainer> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="RconPermissionContainer"/> class.
        /// </summary>
        /// <param name="permissible">Permissible to check the rcon status from.</param>
        /// <param name="logger">Logger used in this container.</param>
        public RconPermissionContainer(IPermissible permissible, ILogger<RconPermissionContainer> logger)
        {
            this.permissible = permissible;
            this.logger = logger;
        }

        /// <inheritdoc />
        public event EventHandler<EventArgs>? PermissionsUpdated;

        /// <inheritdoc />
        public void AttachChildPermissionContainer(IPermissionContainer childPermissionContainer, int tier = 1)
        {
            throw new InvalidOperationException($"Could not attach container {childPermissionContainer.GetType()}, because this only reacts to rcon status.");
        }

        /// <inheritdoc />
        public void RemovePermissionContainer(IPermissionContainer childPermissionContainer, int tier = 1)
        {
            throw new InvalidOperationException($"Could not remove container {childPermissionContainer.GetType()}, because this only reacts to rcon status.");
        }

        /// <inheritdoc />
        public IPermissionAttachment AttachPermission(
            IPermission permission,
            bool value,
            PermissionAttachmentFlag flags = PermissionAttachmentFlag.None,
            int tier = 1,
            IImmutableDictionary<string, string[]>? neededContexts = null)
        {
            throw new InvalidOperationException("Could not attach permission, because this only reacts to rcon status.");
        }

        /// <inheritdoc />
        public IImmutableDictionary<IPermission, bool> GetEffectivePermissions(IImmutableDictionary<string, string> context)
        {
            return new Dictionary<IPermission, bool>().ToImmutableDictionary();
        }

        /// <inheritdoc />
        public bool HasPermission(string permission, IImmutableDictionary<string, string> context)
        {
            switch (this.permissible)
            {
                case IPlayer player:

                    try
                    {
                        return player.IsRconAdmin();
                    }
                    catch (ObjectDisposedException e)
                    {
                        this.logger.LogWarning(e, $"Could not check rcon admin status for {this.permissible}.");
                    }

                    return false;

                default:
                    return false;
            }
        }

        /// <inheritdoc />
        public bool IsPermissionSet(string permission, IImmutableDictionary<string, string> context)
        {
            return true;
        }
    }
}
