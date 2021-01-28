using System;
using System.Drawing;
using System.Linq;
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
        [Command("help", Description = "Shows all available commands")]
        public virtual void HelpCommand(IPlayer player)
        {
            var commandService = (ICommandService)this.serviceProvider.GetRequiredService(typeof(ICommandService));

            player.SendMessage(Color.DeepSkyBlue, string.Empty);
            player.SendMessage(Color.DeepSkyBlue, "| * Available commands");
            player.SendMessage(Color.DeepSkyBlue, "| _________________________________________________");

            foreach (var command in commandService.Commands)
            {
                if (command.Key == string.Empty)
                {
                    continue;
                }

                var availableCommandAmount = command.Value.Count(x => x.Value.AliasNames.Contains(x.Key) == false);

                player.SendMessage(Color.DeepSkyBlue, $"| {Color.White.Embed()}/{command.Key} - {Color.DarkGray.Embed()}{availableCommandAmount} available commands");
            }

            player.SendMessage(Color.DeepSkyBlue, "|");

            if (commandService.Commands.TryGetValue(string.Empty, out var nonGroupedCommands))
            {
                foreach (var nonGroupedCommand in nonGroupedCommands)
                {
                    var description = string.Empty;
                    if (string.IsNullOrWhiteSpace(nonGroupedCommand.Value.Description) == false)
                    {
                        description = $"{Color.DarkGray.Embed()}- {nonGroupedCommand.Value.Description}";
                    }

                    player.SendMessage(Color.DeepSkyBlue, $"| {Color.White.Embed()}/{nonGroupedCommand.Key} {description}");
                }
            }
        }
    }
}
