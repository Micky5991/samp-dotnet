using Micky5991.Samp.Net.Framework.Extensions.FrameworkExtensions.RconPermissions.Services;
using Micky5991.Samp.Net.Framework.Interfaces;
using Micky5991.Samp.Net.Framework.Interfaces.Permissions.Factories;
using Microsoft.Extensions.DependencyInjection;

namespace Micky5991.Samp.Net.Framework.Extensions.FrameworkExtensions.RconPermissions
{
    /// <summary>
    /// Extension that provides permission checks if the player is an rcon admin.
    /// </summary>
    public class RconPermissionExtension : IExtensionBuilder
    {
        /// <inheritdoc />
        public void Register(IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<IPermissionFactory, RconPermissionFactory>();
        }
    }
}
