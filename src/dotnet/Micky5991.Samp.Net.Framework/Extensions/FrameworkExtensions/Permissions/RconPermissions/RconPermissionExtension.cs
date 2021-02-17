using Micky5991.Samp.Net.Framework.Elements.Entities.Listeners;
using Micky5991.Samp.Net.Framework.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace Micky5991.Samp.Net.Framework.Extensions.FrameworkExtensions.Permissions.RconPermissions
{
    /// <summary>
    /// Extension that provides permission checks if the player is an rcon admin.
    /// </summary>
    public class RconPermissionExtension : IExtensionBuilder
    {
        /// <inheritdoc />
        public void Register(IServiceCollection serviceCollection)
        {
            serviceCollection.AddAuthorizationCore(this.ConfigureAuthorization);
        }

        private void ConfigureAuthorization(AuthorizationOptions config)
        {
            config.DefaultPolicy = new AuthorizationPolicyBuilder()
                                   .AddAuthenticationSchemes("SAMP Rcon")
                                   .RequireRole("RconAdmin")
                                   .Build();

            config.FallbackPolicy = new AuthorizationPolicyBuilder()
                                    .RequireAssertion(x => true)
                                    .Build();
        }
    }
}
