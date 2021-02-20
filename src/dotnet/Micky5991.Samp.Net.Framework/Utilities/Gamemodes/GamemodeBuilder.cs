using System;
using System.Collections.Generic;
using System.Linq;
using Micky5991.Samp.Net.Core;
using Micky5991.Samp.Net.Framework.Elements.Gamemodes;
using Micky5991.Samp.Net.Framework.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Micky5991.Samp.Net.Framework.Utilities.Gamemodes
{
    public class GamemodeBuilder
    {
        private readonly IServiceCollection serviceCollection;

        private IStartup startup;

        private List<ISampExtension> extensions;

        public GamemodeBuilder()
            : this(new ServiceCollection())
        {
        }

        public GamemodeBuilder(IServiceCollection serviceCollection)
        {
            this.serviceCollection = serviceCollection;
        }

        public GamemodeBuilder UseStartup<T>()
            where T : class, IStartup, new()
        {
            return this.UseStartup(new T());
        }

        public GamemodeBuilder UseStartup(IStartup startup)
        {
            this.startup = startup;

            return this;
        }

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

            this.startup.SetupConfiguration(builder);

            return builder.Build();
        }

        private void BuildServices(IConfiguration configuration)
        {
            this.serviceCollection.AddSingleton(x => this.startup);

            foreach (var extension in this.extensions)
            {
                extension.RegisterServices(this.serviceCollection, configuration);
            }

            this.startup.RegisterServices(this.serviceCollection, configuration);
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

                                                       this.startup.ConfigureAuthorization(x, configuration);
                                                   });
        }
    }
}
