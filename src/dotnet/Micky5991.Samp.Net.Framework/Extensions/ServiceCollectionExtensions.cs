using System;
using Dawn;
using Micky5991.Samp.Net.Framework.Utilities.Gamemodes;
using Microsoft.Extensions.DependencyInjection;

namespace Micky5991.Samp.Net.Framework.Extensions
{
    /// <summary>
    /// Extensions used to simplify adding services to the service collection.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Uses the default gamemode services builder to add services to the given <paramref name="services"/>.
        /// </summary>
        /// <param name="services">Collection to add the services to.</param>
        /// <returns>Passed <paramref name="services"/> instance.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="services"/> is null.</exception>
        public static IServiceCollection AddSampCoreServices(this IServiceCollection services)
        {
            return services.AddSampCoreServices(new GamemodeServicesBuilder());
        }

        /// <summary>
        /// Uses the given gamemode services builder to add services to the <paramref name="services"/>.
        /// </summary>
        /// <param name="services">Collection to add the services to.</param>
        /// <param name="builder">Builder instance to use for gamemode.</param>
        /// <returns>Passed <paramref name="services"/> instance.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="builder"/> or <paramref name="services"/> is null.</exception>
        public static IServiceCollection AddSampCoreServices(this IServiceCollection services, GamemodeServicesBuilder builder)
        {
            Guard.Argument(services, nameof(services)).NotNull();
            Guard.Argument(builder, nameof(builder)).NotNull();

            builder.AddAllServices(services);

            return services;
        }
    }
}
