using System;
using System.Collections.Generic;
using System.Linq;
using Micky5991.Samp.Net.Framework.Elements.Gamemodes;
using Micky5991.Samp.Net.Framework.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Micky5991.Samp.Net.Framework.Utilities.Gamemodes
{
    /// <summary>
    /// Builder that prepares the <see cref="IServiceCollection"/> and <see cref="IServiceProvider"/> for the SAMP Gamemode.
    /// </summary>
    public class GamemodeBuilder
    {
        private readonly IServiceCollection serviceCollection;

        private IStartup? startup;

        private List<ISampExtension> extensions;

        /// <summary>
        /// Initializes a new instance of the <see cref="GamemodeBuilder"/> class.
        /// </summary>
        public GamemodeBuilder()
            : this(new ServiceCollection())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GamemodeBuilder"/> class.
        /// </summary>
        /// <param name="serviceCollection">Service collection used for new services.</param>
        public GamemodeBuilder(IServiceCollection serviceCollection)
        {
            this.serviceCollection = serviceCollection;
            this.extensions = new List<ISampExtension>();
        }

        /// <summary>
        /// Specifies the specific type of the startup used to boot up the main gamemode.
        /// </summary>
        /// <typeparam name="T">Startup type of the gamemode.</typeparam>
        /// <returns>Current <see cref="GamemodeBuilder"/> instance.</returns>
        public GamemodeBuilder UseStartup<T>()
            where T : class, IStartup, new()
        {
            return this.UseStartup(new T());
        }

        /// <summary>
        /// Specifies the instance of the startup type used to boot up the main gamemode.
        /// </summary>
        /// <param name="newStartup">Instance to use to start gamemode.</param>
        /// <returns>Current <see cref="GamemodeBuilder"/> instance.</returns>
        public GamemodeBuilder UseStartup(IStartup newStartup)
        {
            this.startup = newStartup;

            return this;
        }

        /// <summary>
        /// Builds the <see cref="IServiceProvider"/> for this gamemode.
        /// </summary>
        /// <param name="args">Passed arguments of this commandline.</param>
        /// <returns>Created startable <see cref="IGamemode"/> instance.</returns>
        /// <exception cref="InvalidOperationException"><see cref="startup"/> is null.</exception>
        public virtual IGamemode Build(string[] args)
        {
            if (this.startup == null)
            {
                throw new InvalidOperationException(
                                                    $"You need to provide a {nameof(IStartup)} instance with {nameof(this.UseStartup)} first before build.");
            }

            var config = this.BuildConfiguration(args);
            this.extensions = this.startup.SetupExtensions(config).ToList();

            this.BuildServices(config);
            this.BuildAuthorization(config);

            return new Gamemode(this.serviceCollection.BuildServiceProvider(), config);
        }

        private IConfiguration BuildConfiguration(string[] args)
        {
            var builder = new ConfigurationBuilder();

            builder.AddCommandLine(args);

            this.startup!.SetupConfiguration(builder);

            return builder.Build();
        }

        private void BuildServices(IConfiguration configuration)
        {
            this.serviceCollection.AddSingleton(_ => this.startup!);

            foreach (var extension in this.extensions)
            {
                extension.RegisterServices(this.serviceCollection, configuration);
            }

            this.startup!.RegisterServices(this.serviceCollection, configuration);
        }

        private void BuildAuthorization(IConfiguration configuration)
        {
            this.serviceCollection.AddAuthorizationCore(
                                                   x =>
                                                   {
                                                       foreach (var sampExtension in this.extensions)
                                                       {
                                                           sampExtension.ConfigureAuthorization(x, configuration);
                                                       }

                                                       this.startup!.ConfigureAuthorization(x, configuration);
                                                   });
        }
    }
}
