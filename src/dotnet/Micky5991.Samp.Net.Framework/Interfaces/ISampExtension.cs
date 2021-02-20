using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Micky5991.Samp.Net.Framework.Interfaces
{
    /// <summary>
    /// Represents an extension with provided services.
    /// </summary>
    public interface ISampExtension
    {
        /// <summary>
        /// Registers all extension services to the service collection of the gamemode.
        /// </summary>
        /// <param name="serviceCollection">Service collection of the gamemode.</param>
        void RegisterServices(IServiceCollection serviceCollection, IConfiguration configuration);

        /// <summary>
        /// Configures the authorization of this extension.
        /// </summary>
        /// <param name="options">Options instance that will be configured.</param>
        void ConfigureAuthorization(AuthorizationOptions options, IConfiguration configuration);
    }
}
