using System;
using System.Threading;
using System.Threading.Tasks;
using Micky5991.Samp.Net.Commands.Extensions;
using Micky5991.Samp.Net.Commands.Interfaces;
using Micky5991.Samp.Net.Core.Threading;
using Micky5991.Samp.Net.Example.Commands;
using Micky5991.Samp.Net.Example.Login.Services;
using Micky5991.Samp.Net.Example.Player.Vehicle;
using Micky5991.Samp.Net.Framework.Interfaces;
using Micky5991.Samp.Net.Framework.Options;
using Micky5991.Samp.Net.Framework.Utilities.Gamemodes;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;

namespace Micky5991.Samp.Net.Example
{
    public class Program
    {
        public static void Main(string[] args)
        {
            void ConfigureServices(HostBuilderContext context, IServiceCollection serviceCollection)
            {
                serviceCollection
                    .AddSingleton<IEventListener, ExamplePlayerListener>()
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

            var logger = new LoggerConfiguration()
                         .MinimumLevel.Debug()
                         .WriteTo.Console()
                         .CreateLogger();

            var host = Host.CreateDefaultBuilder()
                           .ConfigureLogging(
                                             options =>
                                             {
                                                 options.ClearProviders();
                                                 options.AddSerilog(logger, true);
                                             })
                           .AddCoreGamemodeServices(
                                                    x =>
                                                    {
                                                        x.LogRedirection = true;
                                                    })
                           .SetFallbackAuthorizationPolicy(true)
                           .AddCommandExtension(
                                                options =>
                                                {
                                                    options.EnableHelpCommand = true;
                                                })
                           .ConfigureServices(ConfigureServices)
                           .Build();

            host.Start();
        }
    }
}
