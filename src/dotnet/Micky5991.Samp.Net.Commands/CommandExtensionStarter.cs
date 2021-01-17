using Micky5991.Samp.Net.Framework.Interfaces;
using Microsoft.Extensions.Logging;

namespace Micky5991.Samp.Net.Commands
{
    /// <summary>
    /// Starts the command extension.
    /// </summary>
    public class CommandExtensionStarter : IExtensionStarter
    {
        private readonly ILogger<CommandExtensionStarter> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandExtensionStarter"/> class.
        /// </summary>
        /// <param name="logger">Logger for this starter instance.</param>
        public CommandExtensionStarter(ILogger<CommandExtensionStarter> logger)
        {
            this.logger = logger;
        }

        /// <inheritdoc/>
        public void Start()
        {
            this.logger.LogInformation("Command extension started.");
        }
    }
}
