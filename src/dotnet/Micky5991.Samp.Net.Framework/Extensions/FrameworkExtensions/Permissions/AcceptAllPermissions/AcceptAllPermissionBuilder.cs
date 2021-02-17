using Micky5991.Samp.Net.Framework.Interfaces;
using Microsoft.AspNetCore.Authorization;
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
            serviceCollection.AddAuthorizationCore(this.ConfigureAuthorization);
        }

        private void ConfigureAuthorization(AuthorizationOptions config)
        {
            config.FallbackPolicy = new AuthorizationPolicyBuilder().RequireAssertion(x => true)
                                                                    .Build();
        }
    }
}
