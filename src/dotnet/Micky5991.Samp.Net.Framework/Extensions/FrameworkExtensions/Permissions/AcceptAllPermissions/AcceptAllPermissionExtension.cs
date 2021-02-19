using System;
using Micky5991.Samp.Net.Framework.Interfaces;
using Micky5991.Samp.Net.Framework.Interfaces.Facades;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace Micky5991.Samp.Net.Framework.Extensions.FrameworkExtensions.Permissions.AcceptAllPermissions
{
    /// <summary>
    /// Registers an permission extension that allows any permission.
    /// </summary>
    public class AcceptAllPermissionExtension : ISampExtension
    {
        /// <inheritdoc />
        public void RegisterServices(IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<ISampExtensionStarter, AcceptAllPermissionStarter>();
        }

        /// <inheritdoc />
        public void ConfigureAuthorization(AuthorizationOptions config)
        {
            var acceptAllPolicy = new AuthorizationPolicyBuilder()
                                  .RequireAssertion(x => true)
                                  .Build();

            config.FallbackPolicy = acceptAllPolicy;
            config.DefaultPolicy = acceptAllPolicy;
        }

        private class AcceptAllPermissionStarter : ISampExtensionStarter
        {
            private readonly IAuthorizationFacade authorizationFacade;

            public AcceptAllPermissionStarter(IAuthorizationFacade authorizationFacade)
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
