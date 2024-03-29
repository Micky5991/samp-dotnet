using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using AutoMapper;
using Dawn;
using Micky5991.Samp.Net.Commands.Elements.CommandHandlers;
using Micky5991.Samp.Net.Commands.Elements.Listeners;
using Micky5991.Samp.Net.Commands.Interfaces;
using Micky5991.Samp.Net.Commands.Services;
using Micky5991.Samp.Net.Framework.Interfaces;
using Micky5991.Samp.Net.Framework.Interfaces.Startup;
using Micky5991.Samp.Net.Framework.Utilities.Startup;
using Microsoft.Extensions.DependencyInjection;

[assembly:InternalsVisibleTo("Micky5991.Samp.Net.Commands.Tests")]

namespace Micky5991.Samp.Net.Commands
{
    /// <summary>
    /// Registers all services for the Commands extension.
    /// </summary>
    public class CommandExtensionBuilder : GamemodeBuilder
    {
        private readonly IList<Assembly> scannableAssemblies = new List<Assembly>();

        private bool addDefaultCommands;

        /// <summary>
        /// Gets a list of scannable assemblies for mapping.
        /// </summary>
        public IReadOnlyList<Assembly> ScannableAssemblies => new ReadOnlyCollection<Assembly>(this.scannableAssemblies);

        /// <summary>
        /// Adds the default mapping profiles of the command extension.
        /// </summary>
        /// <returns>Current <see cref="CommandExtensionBuilder"/> instance.</returns>
        public CommandExtensionBuilder AddDefaultMappingProfiles()
        {
            return this.AddMappingProfilesInAssembly<CommandExtensionBuilder>();
        }

        /// <summary>
        /// Adds an assembly to scan for <see cref="Profile"/> implementations. Recommendation: Use your main type or the <see cref="IGamemodeBuilder"/> implementation for your extension.
        /// </summary>
        /// <typeparam name="T">Any type of an assembly where profiles are created.</typeparam>
        /// <returns>Current <see cref="CommandExtensionBuilder"/> instance.</returns>
        public CommandExtensionBuilder AddMappingProfilesInAssembly<T>()
        {
            return this.AddMappingProfilesInAssembly(typeof(T).Assembly);
        }

        /// <summary>
        /// Adds an assembly to scan for <see cref="Profile"/> implementations.
        /// </summary>
        /// <param name="assembly">Assembly to search for <see cref="Profile"/> implementations.</param>
        /// <returns>Current <see cref="CommandExtensionBuilder"/> instance.</returns>
        public CommandExtensionBuilder AddMappingProfilesInAssembly(Assembly assembly)
        {
            Guard.Argument(assembly, nameof(assembly)).NotNull();

            this.scannableAssemblies.Add(assembly);

            return this;
        }

        /// <summary>
        /// Adds default commands like /help.
        /// </summary>
        /// <returns>Current <see cref="CommandExtensionBuilder"/> instance.</returns>
        public CommandExtensionBuilder AddDefaultCommands()
        {
            this.addDefaultCommands = true;

            return this;
        }

        /// <inheritdoc/>
        public override void RegisterServices(IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<ICommandFactory, CommandFactory>();
            serviceCollection.AddSingleton<ICommandService, CommandService>();
            serviceCollection.AddSingleton<IEventListener, CommandListener>();

            serviceCollection.AddAutoMapper(this.scannableAssemblies.ToArray());

            if (this.addDefaultCommands)
            {
                serviceCollection.AddSingleton<ICommandHandler, HelpCommandHandler>();
            }
        }

        /// <inheritdoc />
        public override void Start(IServiceProvider serviceProvider)
        {
            var starter = serviceProvider.GetRequiredService<ICommandService>();

            starter.Start();
        }
    }
}
