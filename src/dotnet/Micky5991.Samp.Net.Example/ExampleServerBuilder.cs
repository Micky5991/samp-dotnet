using System;
using Micky5991.Samp.Net.Commands.Interfaces;
using Micky5991.Samp.Net.Example.Commands;
using Micky5991.Samp.Net.Example.Login.Services;
using Micky5991.Samp.Net.Example.Player.Vehicle;
using Micky5991.Samp.Net.Framework.Interfaces;
using Micky5991.Samp.Net.Framework.Options;
using Micky5991.Samp.Net.Framework.Utilities.Startup;
using Micky5991.Samp.Net.SerilogSink;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;

namespace Micky5991.Samp.Net.Example
{
    public class ExampleServerBuilder : DefaultServerBuilder
    {
        public override void RegisterServices(IServiceCollection serviceCollection)
        {
            var logger = new LoggerConfiguration()
                         .WriteTo.SampLogFile()
                         .CreateLogger();

            serviceCollection.AddLogging(
                                         builder =>
                                         {
                                             builder.AddSerilog(logger, true);
                                             builder.SetMinimumLevel(LogLevel.Debug);
                                         })
                             .AddSingleton<IEventListener, ExamplePlayerListener>()
                             .AddSingleton<IEventListener, LoginScreen>()
                             .AddSingleton<IEventListener, Speedometer>()
                             .AddSingleton<ICommandHandler, TestCommandHandler>()
                             .AddSingleton<ICommandHandler, VehicleCommandHandler>()
                             .Configure<SampNetOptions>(
                                                         x =>
                                                         {
                                                             x.LogRedirection = false;
                                                         });
        }

        public override void ConfigureAuthorization(AuthorizationOptions options)
        {
            // Configure Authorization
        }
    }
}
