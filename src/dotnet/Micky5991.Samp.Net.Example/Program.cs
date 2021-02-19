using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using Micky5991.Samp.Net.Commands;
using Micky5991.Samp.Net.Commands.Interfaces;
using Micky5991.Samp.Net.Example.Commands;
using Micky5991.Samp.Net.Framework.Extensions;
using Micky5991.Samp.Net.Framework.Extensions.FrameworkExtensions.Permissions.RconPermissions;
using Micky5991.Samp.Net.Framework.Interfaces;
using Micky5991.Samp.Net.NLogTarget;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;

namespace Micky5991.Samp.Net.Example
{
    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                SampLogTarget.Register();

                var serviceProvider = BuildServiceProvider();

                var gamemodeStarter = serviceProvider.GetRequiredService<IGamemodeStarter>();
                var listener = serviceProvider.GetRequiredService<ChatListener>();
                var logger = serviceProvider.GetRequiredService<ILogger<Program>>();

                gamemodeStarter.StartLogRedirection()
                               .Start();

                listener.Attach();

                AppDomain.CurrentDomain.UnhandledException += (sender, eventArgs) =>
                {
                    logger.LogCritical((Exception)eventArgs.ExceptionObject, $"Unhandled exception has been caught from {sender.GetType()}:");
                };
            }
            catch (Exception e)
            {
                Console.WriteLine($"ERROR: {e.Message}");
                Console.WriteLine(e.StackTrace);
            }
        }

        private static IServiceProvider BuildServiceProvider()
        {
            var serviceCollection = new ServiceCollection()
                                    .AddLogging(
                                                builder =>
                                                {
                                                    builder.AddNLog();
                                                    builder.SetMinimumLevel(LogLevel.Debug);
                                                    builder.AddFilter((category, _) => category != typeof(DefaultAuthorizationService).FullName);
                                                })
                                    .AddSampGamemode(
                                                     g => g
                                                          .AddAllServices()
                                                          .AddExtension<CommandExtension>(x => x
                                                              .AddProfilesInAssembly(Assembly.GetExecutingAssembly())
                                                              .AddProfilesInAssembly<CommandExtension>()
                                                              .AddDefaultCommands())
                                                          .AddExtension<RconPermissionExtension>())
                                    .AddSingleton<ChatListener>()
                                    .AddSingleton<ICommandHandler, TestCommandHandler>();

            return serviceCollection.BuildServiceProvider();
        }
    }
}
