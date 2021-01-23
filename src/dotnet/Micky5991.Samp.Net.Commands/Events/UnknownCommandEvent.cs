using System.Collections.Immutable;
using Micky5991.EventAggregator.Elements;
using Micky5991.Samp.Net.Commands.Interfaces;
using Micky5991.Samp.Net.Framework.Interfaces.Entities;

namespace Micky5991.Samp.Net.Commands.Events
{
    /// <summary>
    /// Event which will be triggered when the player enters a command which can't be found. Can also be the case if
    /// there are multiple possible commands. Use <see cref="PotentialCommands"/> to determine if there are no commands
    /// available for that input or to display help to differenciate between multiple commands.
    /// </summary>
    public class UnknownCommandEvent : CancellableEventBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UnknownCommandEvent"/> class.
        /// </summary>
        /// <param name="player">Player that executed the command.</param>
        /// <param name="commandText">Text that has been entered.</param>
        /// <param name="potentialCommands">Potential commands if multiple commands are found.</param>
        /// <param name="groupName">Group name that has been selected.</param>
        /// <param name="remainingCommandString">Remaining input arguments for the actual command.</param>
        public UnknownCommandEvent(IPlayer player, string commandText, IImmutableDictionary<string, ICommand> potentialCommands, string groupName, string remainingCommandString)
        {
            this.Player = player;
            this.CommandText = commandText;
            this.PotentialCommands = potentialCommands;
            this.GroupName = groupName;
            this.RemainingCommandString = remainingCommandString;
        }

        /// <summary>
        /// Gets the player that executed this command.
        /// </summary>
        public IPlayer Player { get; }

        /// <summary>
        /// Gets the full text what the <see cref="Player"/> entered.
        /// </summary>
        public string CommandText { get; }

        /// <summary>
        /// Gets the potential commands which could be available for this <see cref="CommandText"/>.
        /// </summary>
        public IImmutableDictionary<string, ICommand> PotentialCommands { get; }

        /// <summary>
        /// Gets the name of the group which the <see cref="ICommandService"/> was able to find, empty if it was unable
        /// to do so.
        /// </summary>
        public string GroupName { get; }

        /// <summary>
        /// Gets the remaining text of the <see cref="CommandText"/> if there was a command found.
        /// </summary>
        public string RemainingCommandString { get; }
    }
}
