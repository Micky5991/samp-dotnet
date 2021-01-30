using System;
using System.Drawing;
using Micky5991.EventAggregator;
using Micky5991.EventAggregator.Interfaces;
using Micky5991.Samp.Net.Commands.Events;
using Micky5991.Samp.Net.Commands.Interfaces;
using Micky5991.Samp.Net.Framework.Extensions;

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

            if (eventdata.PotentialCommands.Count == 0)
            {
                eventdata.Player.SendMessage(Color.LightGray, "This command does not exist.");

                return;
            }

            eventdata.Player.SendMessage(Color.DeepSkyBlue, string.Empty);

            eventdata.Player.SendMessage(Color.DeepSkyBlue, $"| * Group \"{eventdata.GroupName}\" {Color.LightGray.Embed()}({eventdata.PotentialCommands.Count} commands)");
            eventdata.Player.SendMessage(Color.DeepSkyBlue, "| _________________________________________________");

            foreach (var potentialCommand in eventdata.PotentialCommands)
            {
                var description = string.Empty;
                if (string.IsNullOrWhiteSpace(potentialCommand.Value.Description) == false)
                {
                    description = $"{Color.LightGray.Embed()} - {potentialCommand.Value.Description}";
                }

                eventdata.Player.SendMessage(Color.DeepSkyBlue, $"| {Color.White.Embed()}/{potentialCommand.Key} {description}");
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
                    eventdata.Player.SendMessage(Color.LightGray, "An error occured during command execution. Try again later.");
                    break;

                case CommandExecutionStatus.NoPermission:
                    eventdata.Player.SendMessage(Color.LightGray, string.Empty);
                    eventdata.Player.SendMessage(Color.LightGray, "You don't have the needed permissions to execute this command.");
                    break;

                case CommandExecutionStatus.ArgumentTypeMismatch:
                    eventdata.Player.SendMessage(Color.LightGray, string.Empty);
                    eventdata.Player.SendMessage(Color.LightGray, $"Could not match input to expected types. {(string.IsNullOrWhiteSpace(eventdata.ErrorParameter) == false ? $"Parameter: {eventdata.ErrorParameter}" : string.Empty)}");

                    if (string.IsNullOrWhiteSpace(eventdata.ErrorMessage) == false)
                    {
                        eventdata.Player.SendMessage(Color.LightGray, $"Error: {eventdata.ErrorMessage}");
                    }

                    eventdata.Player.SendMessage(Color.LightGray, $"Use: {eventdata.Command.HelpSignature}");
                    break;

                case CommandExecutionStatus.MissingArgument:
                    eventdata.Player.SendMessage(Color.LightGray, string.Empty);
                    eventdata.Player.SendMessage(Color.LightGray, "The command misses some arguments:");
                    eventdata.Player.SendMessage(Color.LightGray, $"Use: {eventdata.Command.HelpSignature}");

                    break;

                case CommandExecutionStatus.TooManyArguments:
                    eventdata.Player.SendMessage(Color.LightGray, string.Empty);
                    eventdata.Player.SendMessage(Color.LightGray, "You entered too many arguments.");
                    eventdata.Player.SendMessage(Color.LightGray, $"Use: {eventdata.Command.HelpSignature}");
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
