using Micky5991.Samp.Net.Commands.Interfaces;
using Micky5991.Samp.Net.Framework.Interfaces;
using Microsoft.Extensions.Logging;

namespace Micky5991.Samp.Net.Commands.Services
{
    /// <summary>
    /// Starts the command extension.
    /// </summary>
    public class CommandExtensionStarter : IExtensionStarter
    {
        private readonly ILogger<CommandExtensionStarter> logger;

        private readonly ICommandService commandService;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandExtensionStarter"/> class.
        /// </summary>
        /// <param name="logger">Logger for this starter instance.</param>
        /// <param name="commandService">Command service that will be started.</param>
        public CommandExtensionStarter(ILogger<CommandExtensionStarter> logger, ICommandService commandService)
        {
            this.logger = logger;
            this.commandService = commandService;
        }

        /// <inheritdoc/>
        public void Start()
        {
            this.logger.LogInformation("Command extension started.");

            this.commandService.Start();
        }
    }
}
