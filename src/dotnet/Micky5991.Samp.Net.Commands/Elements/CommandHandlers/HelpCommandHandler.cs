using System;
using System.Collections.Immutable;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
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
        public virtual async void HelpCommand(IPlayer player)
        {
            var commandService = (ICommandService)this.serviceProvider.GetRequiredService(typeof(ICommandService));

            player.SendMessage(Color.DeepSkyBlue, string.Empty);
            player.SendMessage(Color.DeepSkyBlue, "| * Available commands");
            player.SendMessage(Color.DeepSkyBlue, "| _________________________________________________");

            var commandGroups = await this.RemoveCommandsWithoutPermissionAsync(player, commandService.NonAliasCommands);

            if (commandGroups.Any(x => string.IsNullOrWhiteSpace(x.Key) == false))
            {
                foreach (var command in commandGroups)
                {
                    if (command.Key == string.Empty)
                    {
                        continue;
                    }

                    player.SendMessage(
                                       Color.DeepSkyBlue,
                                       $"| {Color.White.Embed()}/{command.Key} - {Color.LightGray.Embed()}{command.Value.Count} available commands");
                }

                player.SendMessage(Color.DeepSkyBlue, "|");
            }

            if (commandGroups.TryGetValue(string.Empty, out var nonGroupedCommands))
            {
                foreach (var nonGroupedCommand in nonGroupedCommands)
                {
                    var description = string.Empty;
                    if (string.IsNullOrWhiteSpace(nonGroupedCommand.Value.Description) == false)
                    {
                        description = $"{Color.LightGray.Embed()}- {nonGroupedCommand.Value.Description}";
                    }

                    player.SendMessage(Color.DeepSkyBlue, $"| {Color.White.Embed()}/{nonGroupedCommand.Key} {description}");
                }
            }
        }

        private async Task<IImmutableDictionary<string, IImmutableDictionary<string, ICommand>>>
            RemoveCommandsWithoutPermissionAsync(
                IPlayer player,
                IImmutableDictionary<string, IImmutableDictionary<string, ICommand>> commandGroups)
        {
            var result = commandGroups;

            foreach (var commandGroup in commandGroups)
            {
                var items = await this.RemoveCommandsWithoutPermissionAsync(player, commandGroup.Value);
                if (items.Count == 0)
                {
                    result = result.Remove(commandGroup.Key);

                    continue;
                }

                result = result.SetItem(commandGroup.Key, items);
            }

            return result;
        }

        private async Task<IImmutableDictionary<string, ICommand>> RemoveCommandsWithoutPermissionAsync(
            IPlayer player,
            IImmutableDictionary<string, ICommand> commands)
        {
            var commandTaskMapping = commands
                .ToDictionary(
                              x => x,
                              x => x.Value.CanExecuteCommandAsync(player));

            await Task.WhenAll(commandTaskMapping.Values).ConfigureAwait(false);

            return commandTaskMapping
                          .Where(x => x.Value.Result)
                          .Select(x => x.Key)
                          .ToImmutableDictionary();
        }
    }
}
