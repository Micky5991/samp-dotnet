using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using AutoMapper;
using Dawn;
using Micky5991.Samp.Net.Commands.Elements;
using Micky5991.Samp.Net.Commands.Interfaces;
using Micky5991.Samp.Net.Commands.Services;
using Micky5991.Samp.Net.Framework.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

[assembly:InternalsVisibleTo("Micky5991.Samp.Net.Commands.Tests")]

namespace Micky5991.Samp.Net.Commands
{
    /// <summary>
    /// Registers all services for the Commands extension.
    /// </summary>
    public class CommandExtensionBuilder : IExtensionBuilder
    {
        private readonly IList<Assembly> scannableAssemblies = new List<Assembly>();

        /// <summary>
        /// Gets a list of scannable assemblies for mapping.
        /// </summary>
        public IReadOnlyList<Assembly> ScannableAssemblies => new ReadOnlyCollection<Assembly>(this.scannableAssemblies);

        /// <summary>
        /// Adds an assembly to scan for <see cref="Profile"/> implementations. Recommendation: Use your main type or the <see cref="IExtensionStarter"/> implementation for your extension.
        /// </summary>
        /// <typeparam name="T">Any type of an assembly where profiles are created.</typeparam>
        /// <returns>Current <see cref="CommandExtensionBuilder"/> instance.</returns>
        public CommandExtensionBuilder AddProfilesInAssembly<T>()
        {
            return this.AddProfilesInAssembly(typeof(T).Assembly);
        }

        /// <summary>
        /// Adds an assembly to scan for <see cref="Profile"/> implementations.
        /// </summary>
        /// <param name="assembly">Assembly to search for <see cref="Profile"/> implementations.</param>
        /// <returns>Current <see cref="CommandExtensionBuilder"/> instance.</returns>
        public CommandExtensionBuilder AddProfilesInAssembly(Assembly assembly)
        {
            Guard.Argument(assembly, nameof(assembly)).NotNull();

            this.scannableAssemblies.Add(assembly);

            return this;
        }

        /// <inheritdoc/>
        public void Register(IServiceCollection serviceCollection)
        {
            serviceCollection.TryAddTransient<IExtensionStarter, CommandExtensionStarter>();
            serviceCollection.AddTransient<ICommandFactory, CommandFactory>();
            serviceCollection.AddSingleton<ICommandService, CommandService>();

            serviceCollection.AddAutoMapper(this.scannableAssemblies.ToArray());
        }
    }
}
