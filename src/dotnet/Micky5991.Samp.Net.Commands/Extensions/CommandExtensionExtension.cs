// <copyright file="CommandExtensionExtension.cs" company="Micky5991">
// Copyright (c) Micky5991. All rights reserved.
// </copyright>

using System;
using Micky5991.Samp.Net.Commands.Elements.CommandHandlers;
using Micky5991.Samp.Net.Commands.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Micky5991.Samp.Net.Commands.Extensions
{
    public static class CommandExtensionExtension
    {
        public static IHostBuilder AddCommandExtension(
            this IHostBuilder hostBuilder,
            Action<CommandExtensionOptions>? options = null)
        {
            return hostBuilder.AddCommandExtension(options, new CommandExtensionBuilder());
        }

        public static IHostBuilder AddCommandExtension(
            this IHostBuilder hostBuilder,
            Action<CommandExtensionOptions>? options,
            CommandExtensionBuilder builder)
        {
            hostBuilder.ConfigureServices(
                                          (_, services) =>
                                          {
                                              services.Configure(options);
                                          });

            builder.ConfigureHost(hostBuilder);

            return hostBuilder;
        }

        public static IHostBuilder AddDefaultHelpCommand(this IHostBuilder hostBuilder)
        {
            return hostBuilder
                .ConfigureServices(
                                   (_, services) =>
                                   {
                                       services.AddSingleton<ICommandHandler, HelpCommandHandler>();
                                   });
        }
    }
}
