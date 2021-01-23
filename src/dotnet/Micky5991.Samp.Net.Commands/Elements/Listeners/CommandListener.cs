using System;
using System.Drawing;
using Micky5991.EventAggregator;
using Micky5991.EventAggregator.Interfaces;
using Micky5991.Samp.Net.Commands.Events;
using Micky5991.Samp.Net.Commands.Interfaces;

namespace Micky5991.Samp.Net.Commands.Elements.Listeners
{
    /// <summary>
    /// Starts listening to command related events.
    /// </summary>
    public class CommandListener : ICommandListener
    {
        private readonly IEventAggregator eventAggregator;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandListener"/> class.
        /// </summary>
        /// <param name="eventAggregator">Event aggregator needed for listening.</param>
        public CommandListener(IEventAggregator eventAggregator)
        {
            this.eventAggregator = eventAggregator;
        }

        /// <inheritdoc />
        public void Listen()
        {
            this.eventAggregator.Subscribe<CommandExecutedEvent>(
                                                                 this.OnCommandExecuted,
                                                                 true,
                                                                 threadTarget: ThreadTarget.PublisherThread);

            this.eventAggregator.Subscribe<UnknownCommandEvent>(
                                                                this.OnUnknownCommand,
                                                                true,
                                                                threadTarget: ThreadTarget.PublisherThread);
        }

        private void OnUnknownCommand(UnknownCommandEvent eventdata)
        {
            eventdata.Cancelled = true;

            eventdata.Player.SendMessage(Color.DarkGray, string.Empty);

            if (eventdata.PotentialCommands.Count == 0)
            {
                eventdata.Player.SendMessage(Color.DarkGray, "This command does not exist.");

                return;
            }

            if (string.IsNullOrWhiteSpace(eventdata.GroupName) == false)
            {
                eventdata.Player.SendMessage(Color.DarkGray, $"There are {eventdata.PotentialCommands.Count} commands in the group {eventdata.GroupName}:");
            }
            else
            {
                eventdata.Player.SendMessage(Color.DarkGray, $"There are {eventdata.PotentialCommands.Count} commands available:");
            }

            foreach (var potentialCommand in eventdata.PotentialCommands)
            {
                eventdata.Player.SendMessage(Color.DarkGray, $"/{potentialCommand.Key}");
            }
        }

        private void OnCommandExecuted(CommandExecutedEvent eventdata)
        {
            eventdata.Cancelled = true;

            switch (eventdata.ExecutionStatus)
            {
                case CommandExecutionStatus.Ok:
                    // Do nothing
                    break;

                case CommandExecutionStatus.Exception:
                    break;

                case CommandExecutionStatus.ArgumentTypeMismatch:
                    eventdata.Player.SendMessage(Color.DarkGray, string.Empty);
                    eventdata.Player.SendMessage(Color.DarkGray, "Could not match input to expected types.");
                    eventdata.Player.SendMessage(Color.DarkGray, $"Use: {eventdata.Command.HelpSignature}");
                    break;

                case CommandExecutionStatus.MissingArgument:
                    eventdata.Player.SendMessage(Color.DarkGray, string.Empty);
                    eventdata.Player.SendMessage(Color.DarkGray, "The command misses some arguments:");
                    eventdata.Player.SendMessage(Color.DarkGray, $"Use: {eventdata.Command.HelpSignature}");

                    break;

                case CommandExecutionStatus.TooManyArguments:
                    eventdata.Player.SendMessage(Color.DarkGray, string.Empty);
                    eventdata.Player.SendMessage(Color.DarkGray, "You entered too many arguments.");
                    eventdata.Player.SendMessage(Color.DarkGray, $"Use: {eventdata.Command.HelpSignature}");
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
