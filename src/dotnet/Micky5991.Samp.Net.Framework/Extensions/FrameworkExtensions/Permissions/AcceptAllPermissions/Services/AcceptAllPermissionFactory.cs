using System.Collections.Generic;
using System.Collections.Immutable;
using Micky5991.Samp.Net.Framework.Interfaces.Permissions;
using Micky5991.Samp.Net.Framework.Interfaces.Permissions.Factories;
using Microsoft.Extensions.Logging;

namespace Micky5991.Samp.Net.Framework.Extensions.FrameworkExtensions.Permissions.AcceptAllPermissions.Services
{
    /// <summary>
    /// Factory that creates permission containers that accept all permission request.
    /// </summary>
    public class AcceptAllPermissionFactory : IPermissionFactory
    {
        private readonly ILogger<AcceptAllPermissionContainer> containerLogger;

        /// <summary>
        /// Initializes a new instance of the <see cref="AcceptAllPermissionFactory"/> class.
        /// </summary>
        /// <param name="containerLogger">Logger needed for warnigns in container.</param>
        public AcceptAllPermissionFactory(ILogger<AcceptAllPermissionContainer> containerLogger)
        {
            this.containerLogger = containerLogger;
        }

        /// <inheritdoc />
        public IPermissionContainer BuildContainer(IPermissible? permissible)
        {
            return new AcceptAllPermissionContainer(this.containerLogger);
        }

        /// <inheritdoc />
        public IImmutableDictionary<string, string> CalculateContext(IPermissible permissible)
        {
            return new Dictionary<string, string>().ToImmutableDictionary();
        }
    }
}
