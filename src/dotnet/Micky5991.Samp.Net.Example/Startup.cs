using System;
using System.Collections.Generic;
using System.Reflection;
using Micky5991.Samp.Net.Commands;
using Micky5991.Samp.Net.Commands.Interfaces;
using Micky5991.Samp.Net.Example.Commands;
using Micky5991.Samp.Net.Framework.Extensions;
using Micky5991.Samp.Net.Framework.Extensions.FrameworkExtensions.Permissions.RconPermissions;
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

            yield return new RconPermissionExtension();
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
                .AddSingleton<ChatListener>()
                .AddSingleton<ICommandHandler, TestCommandHandler>()
                .AddSampCoreServices()
                .Configure<GamemodeOptions>(x => x.LogRedirection = true);
        }

        public void ConfigureAuthorization(AuthorizationOptions options, IConfiguration configuration)
        {
        }

        public void Start(IServiceProvider serviceProvider, IConfiguration configuration)
        {
            var chatListener = serviceProvider.GetRequiredService<ChatListener>();
            chatListener.Attach();
        }
    }
}
