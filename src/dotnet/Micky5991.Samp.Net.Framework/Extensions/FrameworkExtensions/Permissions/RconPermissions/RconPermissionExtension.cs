using Micky5991.Samp.Net.Framework.Interfaces;
using Micky5991.Samp.Net.Framework.Interfaces.Facades;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Micky5991.Samp.Net.Framework.Extensions.FrameworkExtensions.Permissions.RconPermissions
{
    /// <summary>
    /// Extension that provides permission checks if the player is an rcon admin.
    /// </summary>
    public class RconPermissionExtension : ISampExtension
    {
        /// <inheritdoc />
        public void RegisterServices(IServiceCollection serviceCollection, IConfiguration configuration)
        {
            serviceCollection.AddTransient<ISampExtensionStarter, RconPermissionSampExtensionStarter>();
        }

        /// <inheritdoc />
        public void ConfigureAuthorization(AuthorizationOptions options, IConfiguration configuration)
        {
            options.DefaultPolicy = new AuthorizationPolicyBuilder()
                                   .AddAuthenticationSchemes("SAMP Rcon")
                                   .RequireRole("RconAdmin")
                                   .Build();

            options.FallbackPolicy = new AuthorizationPolicyBuilder()
                                    .RequireAssertion(x => true)
                                    .Build();
        }

        private class RconPermissionSampExtensionStarter : ISampExtensionStarter
        {
            private readonly IAuthorizationFacade authorizationFacade;

            public RconPermissionSampExtensionStarter(IAuthorizationFacade authorizationFacade)
            {
                this.authorizationFacade = authorizationFacade;
            }

            public void Start()
            {
                this.authorizationFacade.UseDefaultPolicyForUnknownPolicies();
            }
        }
    }
}
