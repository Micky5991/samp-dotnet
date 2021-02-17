using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using Micky5991.Samp.Net.Commands;
using Micky5991.Samp.Net.Commands.Interfaces;
using Micky5991.Samp.Net.Example.Commands;
using Micky5991.Samp.Net.Framework.Extensions.FrameworkExtensions.Permissions.RconPermissions;
using Micky5991.Samp.Net.Framework.Interfaces;
using Micky5991.Samp.Net.Framework.Utilities.Gamemodes;
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

                Console.WriteLine($"Working directory: {Environment.CurrentDirectory}");

                var serviceCollection = new ServiceCollection()
                    .AddLogging(builder =>
                    {
                        builder.SetMinimumLevel(LogLevel.Trace);
                        builder.AddFilter((category,_) => category != typeof(DefaultAuthorizationService).FullName );
                        builder.AddNLog();
                    })
                    .AddSingleton<ChatListener>()
                    .AddSingleton<ICommandHandler, TestCommandHandler>()
                    .AddAuthorizationCore(SetupAuthorization);

                var commandExtensionBuilder = new CommandExtensionBuilder().AddProfilesInAssembly(Assembly.GetExecutingAssembly())
                                                                           .AddProfilesInAssembly<CommandExtensionBuilder>()
                                                                           .AddDefaultCommands();

                new GamemodeBuilder(serviceCollection)
                    .AddExtensionBuilder(commandExtensionBuilder)
                    .AddExtensionBuilder(new RconPermissionExtension())
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

        private static void SetupAuthorization(AuthorizationOptions config)
        {
            config.FallbackPolicy = new AuthorizationPolicyBuilder()
                .RequireAssertion(x => true)
                .Build();

            config.AddPolicy("VehicleCommands", policy => policy.RequireAssertion(x => true));
            config.AddPolicy("TestPolicy", policy => policy.RequireAssertion(y => true));
        }
    }
}
