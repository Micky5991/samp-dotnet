using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;

namespace Micky5991.Samp.Net.Framework.Interfaces
{
    /// <summary>
    /// Describes the starting type for this gamemode.
    /// </summary>
    public interface IStartup : ISampExtension
    {
        /// <summary>
        /// Modifies the configuration builder to include custom providers and files.
        /// </summary>
        /// <param name="configurationBuilder"><paramref name="configurationBuilder"/> used for future configurations.</param>
        public void SetupConfiguration(IConfigurationBuilder configurationBuilder);

        /// <summary>
        /// Returns the list of activated extensions of this gamemode.
        /// </summary>
        /// <param name="configuration">Constructed configuration of this gamemode.</param>
        /// <returns>List of extensions of this gamemode..</returns>
        public IEnumerable<ISampExtension> SetupExtensions(IConfiguration configuration);

        /// <summary>
        /// Starts the gamemode after core services have been already started.
        /// </summary>
        /// <param name="serviceProvider">Current service provider of this gamemode.</param>
        /// <param name="configuration">Constructed configuration of this gamemode.</param>
        void Start(IServiceProvider serviceProvider, IConfiguration configuration);
    }
}
