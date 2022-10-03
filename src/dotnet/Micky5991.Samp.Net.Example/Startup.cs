using System;
using System.Collections.Generic;
using Micky5991.Samp.Net.Commands.Interfaces;
using Micky5991.Samp.Net.Example.Commands;
using Micky5991.Samp.Net.Example.Login.Services;
using Micky5991.Samp.Net.Example.Player.Vehicle;
using Micky5991.Samp.Net.Framework.Interfaces;
using Micky5991.Samp.Net.Framework.Options;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Micky5991.Samp.Net.Example
{
    public class Startup : IStartup
    {
        public void SetupConfiguration(IConfigurationBuilder configurationBuilder)
        {
        }

        public IEnumerable<ISampExtension> SetupExtensions(IConfiguration configuration)
        {
            yield break;
        }

        public void RegisterServices(IServiceCollection serviceCollection, IConfiguration configuration)
        {
            serviceCollection
                .AddSingleton<ExampleStarter>()
                .AddSingleton<IEntityListener, LoginScreen>()
                .AddSingleton<IEntityListener, Speedometer>()
                .AddSingleton<ICommandHandler, TestCommandHandler>()
                .AddSingleton<ICommandHandler, VehicleCommandHandler>()
                .Configure<GamemodeOptions>(x => x.LogRedirection = true);
        }

        public void ConfigureAuthorization(AuthorizationOptions options, IConfiguration configuration)
        {
            options.AddPolicy("VehicleCommands",
                              b =>
                              {
                                  b.RequireRole("RconAdmin");
                              });

            options.FallbackPolicy = new AuthorizationPolicyBuilder()
                                     .RequireAssertion(x => false)
                                     .Build();
        }

        public void Start(IServiceProvider serviceProvider, IConfiguration configuration)
        {
            var chatListener = serviceProvider.GetRequiredService<ExampleStarter>();
            chatListener.Attach();
        }
    }
}
