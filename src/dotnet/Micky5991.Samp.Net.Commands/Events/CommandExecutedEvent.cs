using Micky5991.EventAggregator.Elements;
using Micky5991.Samp.Net.Commands.Data.Results;
using Micky5991.Samp.Net.Commands.Interfaces;
using Micky5991.Samp.Net.Framework.Interfaces.Entities;

namespace Micky5991.Samp.Net.Commands.Events
{
    /// <summary>
    /// Event that will be triggered when a player executed a command.
    /// </summary>
    public class CommandExecutedEvent : CancellableEventBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CommandExecutedEvent"/> class.
        /// </summary>
        /// <param name="player">Player that executed the command.</param>
        /// <param name="result">Result object that describes how the execution resulted.</param>
        /// <param name="errorParameter">Parameter that caused an error.</param>
        /// <param name="command">Executed command.</param>
        public CommandExecutedEvent(IPlayer player, CommandResult result, string? errorParameter, ICommand? command)
        {
            this.Player = player;
            this.Result = result;
            this.Command = command;
            this.ErrorParameter = errorParameter;
        }

        /// <summary>
        /// Gets the player which executed this command.
        /// </summary>
        public IPlayer Player { get; }

        /// <summary>
        /// Gets the result object that describes how the command executed resulted.
        /// </summary>
        public CommandResult Result { get; }

        /// <summary>
        /// Gets the command that has been executed.
        /// </summary>
        public ICommand? Command { get; }

        /// <summary>
        /// Gets the parameter that possibly caused the error.
        /// </summary>
        public string? ErrorParameter { get; }
    }
}
