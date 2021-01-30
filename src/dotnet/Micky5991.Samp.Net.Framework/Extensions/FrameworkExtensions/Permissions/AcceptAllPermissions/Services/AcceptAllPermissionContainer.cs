using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using Dawn;
using Micky5991.Samp.Net.Framework.Elements.Permissions;
using Micky5991.Samp.Net.Framework.Enums.Permissions;
using Micky5991.Samp.Net.Framework.Interfaces.Permissions;
using Microsoft.Extensions.Logging;

namespace Micky5991.Samp.Net.Framework.Extensions.FrameworkExtensions.Permissions.AcceptAllPermissions.Services
{
    /// <summary>
    /// Container that accepts all permission requests.
    /// </summary>
    public class AcceptAllPermissionContainer : IPermissionContainer
    {
        private readonly ILogger<AcceptAllPermissionContainer> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="AcceptAllPermissionContainer"/> class.
        /// </summary>
        /// <param name="logger">Logger needed for warnings while usage.</param>
        public AcceptAllPermissionContainer(ILogger<AcceptAllPermissionContainer> logger)
        {
            this.logger = logger;
        }

        /// <inheritdoc />
        public event EventHandler<EventArgs>? PermissionsUpdated
        {
            add { } remove { }
        }

        /// <inheritdoc />
        public void AttachChildPermissionContainer(IPermissionContainer container, int tier = 1)
        {
            this.logger.LogWarning($"Container {container.GetType()} has been added, but this extension accepts everything.");
        }

        /// <inheritdoc />
        public void RemovePermissionContainer(IPermissionContainer childPermissionContainer, int tier = 1)
        {
            this.logger.LogWarning($"Container {childPermissionContainer.GetType()} has been remove, but this extension accepts everything.");
        }

        /// <inheritdoc />
        public IPermissionAttachment AttachPermission(
            IPermission permission,
            bool value,
            PermissionAttachmentFlag flags = PermissionAttachmentFlag.None,
            int tier = 1,
            IImmutableDictionary<string, string[]>? neededContexts = null)
        {
            Guard.Argument(permission, nameof(permission)).NotNull();

            this.logger.LogWarning($"Permission {permission.GetType()} has been removed, but this extension accepts everything.");

            return new PermissionAttachment(permission, value, flags, neededContexts);
        }

        /// <inheritdoc />
        public IImmutableDictionary<IPermission, bool> GetEffectivePermissions(IImmutableDictionary<string, string> context)
        {
            return new Dictionary<IPermission, bool>().ToImmutableDictionary();
        }

        /// <inheritdoc />
        public bool HasPermission(string permission, IImmutableDictionary<string, string> context)
        {
            return true;
        }

        /// <inheritdoc />
        public bool IsPermissionSet(string permission, IImmutableDictionary<string, string> context)
        {
            return true;
        }
    }
}
