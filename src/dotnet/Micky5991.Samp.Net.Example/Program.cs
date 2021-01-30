using System;
using System.Reflection;
using Micky5991.Samp.Net.Commands;
using Micky5991.Samp.Net.Commands.Interfaces;
using Micky5991.Samp.Net.Example.Commands;
using Micky5991.Samp.Net.Framework.Extensions.FrameworkExtensions.Permissions.AcceptAllPermissions;
using Micky5991.Samp.Net.Framework.Extensions.FrameworkExtensions.Permissions.RconPermissions;
using Micky5991.Samp.Net.Framework.Interfaces;
using Micky5991.Samp.Net.Framework.Utilities.Gamemodes;
using Micky5991.Samp.Net.NLogTarget;
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

                Console.WriteLine($"Working directory: {Environment.CurrentDirectory}");

                var serviceCollection = new ServiceCollection()
                    .AddLogging(builder =>
                    {
                        builder.SetMinimumLevel(LogLevel.Trace);
                        builder.AddNLog();
                    })
                    .AddSingleton<ChatListener>()
                    .AddSingleton<ICommandHandler, TestCommandHandler>();

                var commandExtensionBuilder = new CommandExtensionBuilder().AddProfilesInAssembly(Assembly.GetExecutingAssembly())
                                                                           .AddProfilesInAssembly<CommandExtensionBuilder>()
                                                                           .AddDefaultCommands();

                new GamemodeBuilder(serviceCollection)
                    .AddExtensionBuilder(commandExtensionBuilder)
                    .AddExtensionBuilder(new AcceptAllPermissionBuilder())
                    .AddCoreServices();

                var serviceProvider = serviceCollection.BuildServiceProvider();

                serviceProvider.GetRequiredService<IGamemodeStarter>()
                               .StartLogRedirection()
                               .Start();

                var listener = serviceProvider.GetRequiredService<ChatListener>();
                listener.Attach();

                var logger = serviceProvider.GetRequiredService<ILogger<Program>>();

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
    }
}
