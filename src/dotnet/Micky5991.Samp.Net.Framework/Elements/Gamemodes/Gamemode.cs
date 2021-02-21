using System;
using Micky5991.Samp.Net.Core.Interfaces.Interop;
using Micky5991.Samp.Net.Framework.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Micky5991.Samp.Net.Framework.Elements.Gamemodes
{
    /// <inheritdoc />
    public class Gamemode : IGamemode
    {
        private readonly IConfiguration configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="Gamemode"/> class.
        /// </summary>
        /// <param name="serviceProvider">Created service provider of this gamemode.</param>
        /// <param name="configuration">Constructed configuration of this gamemode.</param>
        public Gamemode(IServiceProvider serviceProvider, IConfiguration configuration)
        {
            this.configuration = configuration;
            this.ServiceProvider = serviceProvider;
        }

        /// <inheritdoc />
        public IServiceProvider ServiceProvider { get; }

        /// <inheritdoc />
        public void Start()
        {
            var threadEnforcer = this.ServiceProvider.GetRequiredService<ISampThreadEnforcer>();
            threadEnforcer.SetMainThread();

            var gamemodeStarter = this.ServiceProvider.GetRequiredService<IGamemodeStarter>();
            var startup = this.ServiceProvider.GetRequiredService<IStartup>();

            gamemodeStarter.Start();
            startup.Start(this.ServiceProvider, this.configuration);
        }
    }
}
