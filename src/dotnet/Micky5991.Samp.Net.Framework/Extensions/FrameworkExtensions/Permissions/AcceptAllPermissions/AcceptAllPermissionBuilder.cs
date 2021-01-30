using Micky5991.Samp.Net.Framework.Extensions.FrameworkExtensions.Permissions.AcceptAllPermissions.Services;
using Micky5991.Samp.Net.Framework.Interfaces;
using Micky5991.Samp.Net.Framework.Interfaces.Permissions.Factories;
using Microsoft.Extensions.DependencyInjection;

namespace Micky5991.Samp.Net.Framework.Extensions.FrameworkExtensions.Permissions.AcceptAllPermissions
{
    /// <summary>
    /// Registers an permission extension that allows any permission.
    /// </summary>
    public class AcceptAllPermissionBuilder : IExtensionBuilder
    {
        /// <inheritdoc />
        public void Register(IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<IPermissionFactory, AcceptAllPermissionFactory>();
        }
    }
}
