using Micky5991.Samp.Net.Framework.Options;
using Micky5991.Samp.Net.Framework.Utilities.Startup;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace Micky5991.Samp.Net.Framework.Extensions.FrameworkExtensions.Permissions.RconPermissions
{
    /// <summary>
    /// Extension that provides permission checks if the player is an rcon admin.
    /// </summary>
    public class RconPermissionExtensionBuilder : GamemodeBuilder
    {
        /// <inheritdoc />
        public override void RegisterServices(IServiceCollection serviceCollection)
        {
            serviceCollection.Configure<SampNetOptions>(x => x.UseDefaultPolicyForUnknownPolicy = true);
        }

        /// <inheritdoc />
        public override void ConfigureAuthorization(AuthorizationOptions options)
        {
            options.DefaultPolicy = new AuthorizationPolicyBuilder()
                                   .AddAuthenticationSchemes("SAMP Rcon")
                                   .RequireRole("RconAdmin")
                                   .Build();

            options.FallbackPolicy = new AuthorizationPolicyBuilder()
                                    .RequireAssertion(x => true)
                                    .Build();
        }
    }
}
