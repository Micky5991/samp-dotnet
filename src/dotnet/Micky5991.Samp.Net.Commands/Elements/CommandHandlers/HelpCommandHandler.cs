using System;
using System.Drawing;
using Micky5991.Samp.Net.Commands.Attributes;
using Micky5991.Samp.Net.Commands.Interfaces;
using Micky5991.Samp.Net.Framework.Extensions;
using Micky5991.Samp.Net.Framework.Interfaces.Entities;
using Microsoft.Extensions.DependencyInjection;

namespace Micky5991.Samp.Net.Commands.Elements.CommandHandlers
{
    /// <summary>
    /// Registers commands related to help.
    /// </summary>
    public class HelpCommandHandler : ICommandHandler
    {
        private readonly IServiceProvider serviceProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="HelpCommandHandler"/> class.
        /// </summary>
        /// <param name="serviceProvider">General service provider needed to resolve <see cref="ICommandService"/>.</param>
        public HelpCommandHandler(IServiceProvider serviceProvider)
        {
            // We use the service provider directly to avoid circular dependency to ICommandService.
            this.serviceProvider = serviceProvider;
        }

        /// <summary>
        /// Command that shows available commands.
        /// </summary>
        /// <param name="player">Player that executed this command.</param>
        [Command("help")]
        public void HelpCommand(IPlayer player)
        {
            var commandService = (ICommandService)this.serviceProvider.GetRequiredService(typeof(ICommandService));

            player.SendMessage(Color.White, string.Empty);
            player.SendMessage(Color.White, "== [ Available groups ] ==");
            foreach (var command in commandService.Commands)
            {
                if (command.Key == string.Empty)
                {
                    continue;
                }

                player.SendMessage(Color.White, $"/{command.Key} - {Color.DarkGray.Embed()}{command.Value.Count} available commands");
            }

            player.SendMessage(Color.White, string.Empty);

            if (commandService.Commands.TryGetValue(string.Empty, out var nonGroupedCommands))
            {
                foreach (var nonGroupedCommand in nonGroupedCommands)
                {
                    player.SendMessage(Color.White, $"/{nonGroupedCommand.Key}");
                }
            }
        }
    }
}
