using System;
using System.Collections.Generic;
using System.Reflection;
using Micky5991.Samp.Net.Commands;
using Micky5991.Samp.Net.Commands.Interfaces;
using Micky5991.Samp.Net.Example.Commands;
using Micky5991.Samp.Net.Example.Login.Services;
using Micky5991.Samp.Net.Example.Player.Vehicle;
using Micky5991.Samp.Net.Framework.Extensions;
using Micky5991.Samp.Net.Framework.Extensions.FrameworkExtensions.Permissions.AcceptAllPermissions;
using Micky5991.Samp.Net.Framework.Interfaces;
using Micky5991.Samp.Net.Framework.Options;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;

namespace Micky5991.Samp.Net.Example
{
    public class Startup : IStartup
    {
        public void SetupConfiguration(IConfigurationBuilder configurationBuilder)
        {
        }

        public IEnumerable<ISampExtension> SetupExtensions(IConfiguration configuration)
        {
            yield return new CommandExtension()
                         .AddProfilesInAssembly<CommandExtension>()
                         .AddProfilesInAssembly(Assembly.GetExecutingAssembly())
                         .AddDefaultCommands();

            yield return new AcceptAllPermissionExtension();
        }

        public void RegisterServices(IServiceCollection serviceCollection, IConfiguration configuration)
        {
            serviceCollection
                .AddLogging(
                            builder =>
                            {
                                builder.AddNLog();
                                builder.SetMinimumLevel(LogLevel.Debug);
                                builder.AddFilter(
                                                  (category, _) =>
                                                      category != typeof(DefaultAuthorizationService).FullName);
                            })
                .AddSingleton<ExampleStarter>()
                .AddSingleton<IEntityListener, LoginScreen>()
                .AddSingleton<IEntityListener, Speedometer>()
                .AddSingleton<ICommandHandler, TestCommandHandler>()
                .AddSingleton<ICommandHandler, VehicleCommandHandler>()
                .AddSampCoreServices()
                .Configure<GamemodeOptions>(x => x.LogRedirection = true);
        }

        public void ConfigureAuthorization(AuthorizationOptions options, IConfiguration configuration)
        {
            // options.AddPolicy("VehicleCommands",
            //                   b =>
            //                   {
            //                       b.RequireRole("RconAdmin");
            //                   });
        }

        public void Start(IServiceProvider serviceProvider, IConfiguration configuration)
        {
            var chatListener = serviceProvider.GetRequiredService<ExampleStarter>();
            chatListener.Attach();
        }
    }
}
