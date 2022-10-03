using Micky5991.Samp.Net.Framework.Utilities.Startup;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;

namespace Micky5991.Samp.Net.Example
{
    public class ExampleServerBuilder : DefaultServerBuilder
    {
        public override void RegisterServices(IServiceCollection serviceCollection)
        {
            serviceCollection.AddLogging(
                                         builder =>
                                         {
                                             builder.AddNLog();
                                             builder.SetMinimumLevel(LogLevel.Debug);
                                         });
        }

        public override void ConfigureAuthorization(AuthorizationOptions options)
        {
            // Configure Authorization
        }
    }
}
