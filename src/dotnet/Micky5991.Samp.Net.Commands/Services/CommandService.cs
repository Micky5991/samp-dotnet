using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text.RegularExpressions;
using AutoMapper;
using Dawn;
using Micky5991.EventAggregator;
using Micky5991.EventAggregator.Interfaces;
using Micky5991.Samp.Net.Commands.Events;
using Micky5991.Samp.Net.Commands.Exceptions;
using Micky5991.Samp.Net.Commands.Interfaces;
using Micky5991.Samp.Net.Framework.Events.Samp;
using Microsoft.Extensions.Logging;

namespace Micky5991.Samp.Net.Commands.Services
{
    /// <inheritdoc />
    public class CommandService : ICommandService
    {
        private readonly IMapper mapper;

        private readonly IEventAggregator eventAggregator;

        private readonly ICommandFactory commandFactory;

        private readonly ILogger<CommandService> logger;

        private readonly ICollection<ICommandHandler> commandHandlers;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandService"/> class.
        /// </summary>
        /// <param name="mapper">Mapper that converts parameters to actual values.</param>
        /// <param name="eventAggregator">EventAggregator that listens to certain command events.</param>
        /// <param name="commandFactory">Factory that creates single commands from an <see cref="ICommandHandler"/> instance.</param>
        /// <param name="logger">Logger used in this service.</param>
        /// <param name="commandHandlers">Commandhandlers to register into this command service.</param>
        [SuppressMessage("ReSharper", "PossibleMultipleEnumeration", Justification = "Guard does not iterate enumerator")]
        public CommandService(
            IMapper mapper,
            IEventAggregator eventAggregator,
            ICommandFactory commandFactory,
            ILogger<CommandService> logger,
            IEnumerable<ICommandHandler> commandHandlers)
        {
            Guard.Argument(mapper, nameof(mapper)).NotNull();
            Guard.Argument(eventAggregator, nameof(eventAggregator)).NotNull();
            Guard.Argument(commandFactory, nameof(commandFactory)).NotNull();
            Guard.Argument(logger, nameof(logger)).NotNull();
            Guard.Argument(commandHandlers, nameof(commandHandlers)).NotNull();

            this.mapper = mapper;
            this.eventAggregator = eventAggregator;
            this.commandFactory = commandFactory;
            this.logger = logger;
            this.commandHandlers = commandHandlers.ToList();

            this.Commands = new Dictionary<string, IImmutableDictionary<string, ICommand>>().ToImmutableDictionary();
        }

        /// <inheritdoc />
        public IImmutableDictionary<string, IImmutableDictionary<string, ICommand>> Commands { get; private set; }

        /// <inheritdoc/>
        public void Start()
        {
            this.eventAggregator.Subscribe<PlayerCommandEvent>(this.OnPlayerCommand, true, threadTarget: ThreadTarget.PublisherThread);

            this.Commands = this.LoadCommands()
                                .ToDictionary(
                                              x => x.Key,
                                              x => (IImmutableDictionary<string, ICommand>)x.Value.ToImmutableDictionary())
                                .ToImmutableDictionary();

            this.logger.LogInformation($"{this.Commands.Sum(x => x.Value.Count)} commands have been loaded");
        }

        internal void OnPlayerCommand(PlayerCommandEvent eventdata)
        {
            Guard.Argument(eventdata, nameof(eventdata)).NotNull();

            eventdata.Cancelled = true;

            if (this.TryGetCommandFromArgumentText(
                                                   eventdata.CommandText.Substring(1),
                                                   out var potentialCommands,
                                                   out var groupName,
                                                   out var remainingArgumentText) == false)
            {
                this.eventAggregator.Publish(
                                             new UnknownCommandEvent(
                                                                     eventdata.Player,
                                                                     eventdata.CommandText,
                                                                     potentialCommands.ToImmutableDictionary(),
                                                                     groupName,
                                                                     remainingArgumentText));

                return;
            }

            void ApplyContext(IMappingOperationOptions options)
            {
                options.Items[CommandConstants.CommandExecutorKey] = eventdata.Player;
            }

            var command = potentialCommands.Values.First();
            string[] argumentParts = this.SplitArgumentStringToArgumentList(remainingArgumentText, command.Parameters.Count - 1);
            object[] argumentValues = this.MapArgumentListToTypes(
                                                                  argumentParts,
                                                                  command.Parameters.Skip(1).Select(x => x.Type)
                                                                         .ToList(),
                                                                  ApplyContext);

            var status = command.TryExecute(eventdata.Player, argumentValues, out var errorMessage);

            this.eventAggregator.Publish(new CommandExecutedEvent(eventdata.Player, status, errorMessage, command));
        }

        internal bool TryGetCommandFromArgumentText(
            string argumentText,
            out IDictionary<string, ICommand> potentialCommands,
            out string groupName,
            out string remainingArgumentText)
        {
            Guard.Argument(argumentText, nameof(argumentText)).NotNull().NotWhiteSpace();

            var argumentParts = argumentText.Split(new[] { ' ' }, 3);

            groupName = string.Empty;

            // The argumentText only contains group or a groupless verb.
            if (argumentParts.Length == 1)
            {
                // If this is a group, but without a verb, return potential commands.
                if (this.Commands.TryGetValue(argumentParts[0], out var groupCommands))
                {
                    var group = argumentParts[0];
                    groupName = group;

                    remainingArgumentText = string.Empty;
                    potentialCommands = groupCommands.ToDictionary(x => $"{group} {x.Key}", x => x.Value);

                    return false;
                }

                if (this.Commands.TryGetValue(string.Empty, out groupCommands))
                {
                    groupName = string.Empty;

                    if (groupCommands.TryGetValue(argumentParts[0], out var command))
                    {
                        remainingArgumentText = string.Empty;
                        potentialCommands = new Dictionary<string, ICommand>
                        {
                            { argumentParts[0], command },
                        };

                        return true;
                    }
                }
            }

            if (argumentParts.Length >= 2)
            {
                if (this.Commands.TryGetValue(argumentParts[0], out var groupCommands) &&
                    groupCommands.TryGetValue(argumentParts[1], out var command))
                {
                    groupName = command.Group!;

                    remainingArgumentText = string.Join(" ", argumentParts.Skip(2));
                    potentialCommands = new Dictionary<string, ICommand>
                    {
                        { $"{command.Group} {command.Name}", command },
                    };

                    return true;
                }

                if (this.Commands.TryGetValue(string.Empty, out groupCommands) &&
                    groupCommands.TryGetValue(argumentParts[0], out command))
                {
                    remainingArgumentText = string.Join(" ", argumentParts.Skip(1));
                    potentialCommands = new Dictionary<string, ICommand>
                    {
                        { command.Name, command },
                    };

                    return true;
                }
            }

            remainingArgumentText = argumentText;
            potentialCommands = new Dictionary<string, ICommand>();

            return false;
        }

        internal string[] SplitArgumentStringToArgumentList(string argumentString, int argumentAmount)
        {
            Guard.Argument(argumentAmount, nameof(argumentAmount)).NotNegative();

            argumentString = Regex.Replace(argumentString.Trim(), @"\s+", " ");

            if (argumentAmount == 0 || string.IsNullOrWhiteSpace(argumentString))
            {
                return Array.Empty<string>();
            }

            return argumentString.Split(new[] { ' ' }, argumentAmount);
        }

        internal object[] MapArgumentListToTypes(string[] arguments, IList<Type> types, Action<IMappingOperationOptions<object, object>> contextApplicator)
        {
            Guard.Argument(arguments, nameof(arguments)).NotNull();
            Guard.Argument(types, nameof(types)).NotNull();

            Guard.Argument(arguments, nameof(arguments)).MaxCount(types.Count);

            Guard.Argument(contextApplicator, nameof(contextApplicator)).NotNull();

            var result = new object[arguments.Length];

            for (var i = 0; i < arguments.Length; i++)
            {
                result[i] = this.mapper.Map(arguments[i], typeof(string), types[i], contextApplicator);
            }

            return result;
        }

        private Dictionary<string, Dictionary<string, ICommand>> LoadCommands()
        {
            var result = new Dictionary<string, Dictionary<string, ICommand>>();

            foreach (var commandHandler in this.commandHandlers)
            {
                var createdCommands = this.commandFactory.BuildFromCommandHandler(commandHandler);

                foreach (var command in createdCommands)
                {
                    var groupName = (command.Group ?? string.Empty).Trim();

                    if (result.TryGetValue(groupName, out var groupCommands) == false)
                    {
                        result[groupName] = new Dictionary<string, ICommand>
                        {
                            { command.Name, command },
                        };

                        foreach (var aliasName in command.AliasNames)
                        {
                            result[groupName][aliasName] = command;
                        }

                        continue;
                    }

                    var commandName = command.Name.Trim();

                    if (groupCommands.TryGetValue(commandName, out _))
                    {
                        throw new DuplicateCommandException(commandName, groupName);
                    }

                    groupCommands[commandName] = command;
                }
            }

            return result;
        }
    }
}
