using Micky5991.Samp.Net.Framework.Options;
using Micky5991.Samp.Net.Framework.Utilities.Startup;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace Micky5991.Samp.Net.Framework.Extensions.FrameworkExtensions.Permissions.AcceptAllPermissions
{
    /// <summary>
    /// Registers an permission extension that allows any permission.
    /// </summary>
    public class AcceptAllPermissionExtension : GamemodeBuilder
    {
        /// <inheritdoc />
        public override void RegisterServices(IServiceCollection serviceCollection)
        {
            serviceCollection.Configure<SampNetOptions>(x => x.UseDefaultPolicyForUnknownPolicy = true);
        }

        /// <inheritdoc />
        public override void ConfigureAuthorization(AuthorizationOptions options)
        {
            var acceptAllPolicy = new AuthorizationPolicyBuilder()
                                  .RequireAssertion(x => true)
                                  .Build();

            options.FallbackPolicy = acceptAllPolicy;
            options.DefaultPolicy = acceptAllPolicy;
        }
    }
}
