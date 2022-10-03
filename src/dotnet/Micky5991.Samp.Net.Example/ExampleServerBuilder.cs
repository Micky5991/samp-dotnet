using System;
using Micky5991.Samp.Net.Commands.Interfaces;
using Micky5991.Samp.Net.Example.Commands;
using Micky5991.Samp.Net.Example.Login.Services;
using Micky5991.Samp.Net.Example.Player.Vehicle;
using Micky5991.Samp.Net.Framework.Interfaces;
using Micky5991.Samp.Net.Framework.Options;
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
                                         })
                             .AddSingleton<ExamplePlayerListener>()
                             .AddSingleton<IEventListener, LoginScreen>()
                             .AddSingleton<IEventListener, Speedometer>()
                             .AddSingleton<ICommandHandler, TestCommandHandler>()
                             .AddSingleton<ICommandHandler, VehicleCommandHandler>()
                             .Configure<SampNetOptions>(
                                                         x =>
                                                         {
                                                             x.LogRedirection = true;
                                                         });
        }

        public override void ConfigureAuthorization(AuthorizationOptions options)
        {
            // Configure Authorization
        }
    }
}
