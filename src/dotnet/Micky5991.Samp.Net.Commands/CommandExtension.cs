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
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

[assembly:InternalsVisibleTo("Micky5991.Samp.Net.Commands.Tests")]

namespace Micky5991.Samp.Net.Commands
{
    /// <summary>
    /// Registers all services for the Commands extension.
    /// </summary>
    public class CommandExtension : ISampExtension
    {
        private readonly IList<Assembly> scannableAssemblies = new List<Assembly>();

        private readonly IList<Action<IServiceCollection>> serviceCollectionChanges;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandExtension"/> class.
        /// </summary>
        public CommandExtension()
        {
            this.serviceCollectionChanges = new List<Action<IServiceCollection>>();
        }

        /// <summary>
        /// Gets a list of scannable assemblies for mapping.
        /// </summary>
        public IReadOnlyList<Assembly> ScannableAssemblies => new ReadOnlyCollection<Assembly>(this.scannableAssemblies);

        /// <summary>
        /// Adds an assembly to scan for <see cref="Profile"/> implementations. Recommendation: Use your main type or the <see cref="ISampExtensionStarter"/> implementation for your extension.
        /// </summary>
        /// <typeparam name="T">Any type of an assembly where profiles are created.</typeparam>
        /// <returns>Current <see cref="CommandExtension"/> instance.</returns>
        public CommandExtension AddProfilesInAssembly<T>()
        {
            return this.AddProfilesInAssembly(typeof(T).Assembly);
        }

        /// <summary>
        /// Adds an assembly to scan for <see cref="Profile"/> implementations.
        /// </summary>
        /// <param name="assembly">Assembly to search for <see cref="Profile"/> implementations.</param>
        /// <returns>Current <see cref="CommandExtension"/> instance.</returns>
        public CommandExtension AddProfilesInAssembly(Assembly assembly)
        {
            Guard.Argument(assembly, nameof(assembly)).NotNull();

            this.scannableAssemblies.Add(assembly);

            return this;
        }

        /// <summary>
        /// Adds default commands like /help.
        /// </summary>
        /// <returns>Current <see cref="CommandExtension"/> instance.</returns>
        public CommandExtension AddDefaultCommands()
        {
            this.serviceCollectionChanges.Add(
                                              x =>
                                              {
                                                  x.AddSingleton<ICommandHandler, HelpCommandHandler>();
                                              });

            return this;
        }

        /// <inheritdoc/>
        public void RegisterServices(IServiceCollection serviceCollection, IConfiguration configuration)
        {
            serviceCollection.TryAddTransient<ISampExtensionStarter, CommandExtensionStarter>();
            serviceCollection.AddTransient<ICommandFactory, CommandFactory>();
            serviceCollection.AddSingleton<ICommandService, CommandService>();
            serviceCollection.AddSingleton<ICommandListener, CommandListener>();

            serviceCollection.AddAutoMapper(this.scannableAssemblies.ToArray());

            foreach (var collectionChange in this.serviceCollectionChanges)
            {
                collectionChange(serviceCollection);
            }
        }

        /// <inheritdoc />
        public void ConfigureAuthorization(AuthorizationOptions options, IConfiguration configuration)
        {
            // Empty
        }
    }
}
