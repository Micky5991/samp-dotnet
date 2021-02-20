using System;
using Micky5991.Samp.Net.Core.Interfaces.Interop;
using Micky5991.Samp.Net.Framework.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Micky5991.Samp.Net.Framework.Elements.Gamemodes
{
    public class Gamemode : IGamemode
    {
        private readonly IConfiguration configuration;

        public IServiceProvider ServiceProvider { get; }

        public Gamemode(IServiceProvider serviceProvider, IConfiguration configuration)
        {
            this.configuration = configuration;
            this.ServiceProvider = serviceProvider;
        }

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
