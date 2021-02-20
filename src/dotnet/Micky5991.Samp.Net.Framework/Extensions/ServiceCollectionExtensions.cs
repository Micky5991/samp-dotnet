using Dawn;
using Micky5991.Samp.Net.Framework.Utilities.Gamemodes;
using Microsoft.Extensions.DependencyInjection;

namespace Micky5991.Samp.Net.Framework.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSampCoreServices(this IServiceCollection services)
        {
            return services.AddSampCoreServices(new GamemodeServicesBuilder());
        }

        public static IServiceCollection AddSampCoreServices(this IServiceCollection services, GamemodeServicesBuilder builder)
        {
            Guard.Argument(services, nameof(services)).NotNull();

            builder.AddAllServices(services);

            return services;
        }
    }
}
