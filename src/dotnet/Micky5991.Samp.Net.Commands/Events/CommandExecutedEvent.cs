using Micky5991.EventAggregator.Elements;
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
        /// <param name="executionStatus">Describing status code of this command error.</param>
        /// <param name="errorMessage">Error message built by the service.</param>
        /// <param name="command">Executed command.</param>
        public CommandExecutedEvent(IPlayer player, CommandExecutionStatus executionStatus, string? errorMessage, ICommand command)
        {
            this.Player = player;
            this.ExecutionStatus = executionStatus;
            this.ErrorMessage = errorMessage;
            this.Command = command;
        }

        /// <summary>
        /// Gets the player which executed this command.
        /// </summary>
        public IPlayer Player { get; }

        /// <summary>
        /// Gets the command that has been executed.
        /// </summary>
        public ICommand Command { get; }

        /// <summary>
        /// Gets the status after trying to execute a command.
        /// </summary>
        public CommandExecutionStatus ExecutionStatus { get; }

        /// <summary>
        /// Gets the error message that was passed for this <see cref="ExecutionStatus"/>.
        /// </summary>
        public string? ErrorMessage { get; }
    }
}
