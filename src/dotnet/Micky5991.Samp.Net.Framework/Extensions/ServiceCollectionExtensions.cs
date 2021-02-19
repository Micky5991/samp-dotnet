using System;
using Dawn;
using Micky5991.Samp.Net.Framework.Interfaces;
using Micky5991.Samp.Net.Framework.Utilities.Gamemodes;
using Microsoft.Extensions.DependencyInjection;

namespace Micky5991.Samp.Net.Framework.Extensions
{
    /// <summary>
    /// Extension to streamline the gamemode registration to the dependency injection container.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Registers all neeeded services for a SA:MP gamemode to the service container.
        /// </summary>
        /// <param name="serviceCollection">Collection where the services will be added to.</param>
        /// <param name="configuration">Configuration of the gamemode builder.</param>
        /// <returns>Current <see cref="IServiceCollection"/> instance.</returns>
        public static IServiceCollection AddSampGamemode(
            this IServiceCollection serviceCollection,
            Action<GamemodeBuilder> configuration)
        {
            Guard.Argument(serviceCollection, nameof(serviceCollection)).NotNull();
            Guard.Argument(configuration, nameof(configuration)).NotNull();

            var builder = new GamemodeBuilder(serviceCollection);

            configuration(builder);

            return serviceCollection;
        }

        /// <summary>
        /// Sets the default SAMP gamemode dependencies and services.
        /// </summary>
        /// <param name="serviceCollection">Collection where the services will be added to.</param>
        /// <returns>Current <see cref="IServiceCollection"/> instance.</returns>
        public static IServiceCollection AddSampGamemode(this IServiceCollection serviceCollection)
        {
            Guard.Argument(serviceCollection, nameof(serviceCollection)).NotNull();

            var builder = new GamemodeBuilder(serviceCollection);

            builder.AddAllServices();

            return serviceCollection;
        }
    }
}
