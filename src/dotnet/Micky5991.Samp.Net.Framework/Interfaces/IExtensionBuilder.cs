using Microsoft.Extensions.DependencyInjection;

namespace Micky5991.Samp.Net.Framework.Interfaces
{
    /// <summary>
    /// Represents an extension with provided services.
    /// </summary>
    public interface IExtensionBuilder
    {
        /// <summary>
        /// Registers all extension services to the service collection of the gamemode.
        /// </summary>
        /// <param name="serviceCollection">Service collection of the gamemode.</param>
        void Register(IServiceCollection serviceCollection);
    }
}
