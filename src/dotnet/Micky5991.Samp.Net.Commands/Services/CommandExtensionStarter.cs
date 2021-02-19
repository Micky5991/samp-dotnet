using System.Collections.Generic;
using Micky5991.Samp.Net.Commands.Interfaces;
using Micky5991.Samp.Net.Framework.Interfaces;
using Microsoft.Extensions.Logging;

namespace Micky5991.Samp.Net.Commands.Services
{
    /// <summary>
    /// Starts the command extension.
    /// </summary>
    public class CommandExtensionStarter : ISampExtensionStarter
    {
        private readonly ILogger<CommandExtensionStarter> logger;

        private readonly ICommandService commandService;

        private readonly IEnumerable<ICommandListener> commandListeners;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandExtensionStarter"/> class.
        /// </summary>
        /// <param name="logger">Logger for this starter instance.</param>
        /// <param name="commandService">Command service that will be started.</param>
        /// <param name="commandListeners">Command listeners that will be started before the <see cref="ICommandService"/>.</param>
        public CommandExtensionStarter(ILogger<CommandExtensionStarter> logger, ICommandService commandService, IEnumerable<ICommandListener> commandListeners)
        {
            this.logger = logger;
            this.commandService = commandService;
            this.commandListeners = commandListeners;
        }

        /// <inheritdoc/>
        public void Start()
        {
            this.logger.LogInformation("Starting command extension...");

            foreach (var commandListener in this.commandListeners)
            {
                commandListener.Listen();
            }

            this.commandService.Start();

            this.logger.LogInformation("Command extension has been started.");
        }
    }
}
