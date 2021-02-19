using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AutoMapper;
using Dawn;
using Micky5991.EventAggregator;
using Micky5991.EventAggregator.Interfaces;
using Micky5991.Samp.Net.Commands.Data.Results;
using Micky5991.Samp.Net.Commands.Events;
using Micky5991.Samp.Net.Commands.Exceptions;
using Micky5991.Samp.Net.Commands.Interfaces;
using Micky5991.Samp.Net.Framework.Events.Samp;
using Micky5991.Samp.Net.Framework.Interfaces.Entities;
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
            this.NonAliasCommands = this.Commands;
        }

        /// <inheritdoc />
        public IImmutableDictionary<string, IImmutableDictionary<string, ICommand>> Commands { get; private set; }

        /// <inheritdoc />
        public IImmutableDictionary<string, IImmutableDictionary<string, ICommand>> NonAliasCommands { get; private set; }

        /// <inheritdoc/>
        public void Start()
        {
            this.eventAggregator.Subscribe<PlayerCommandEvent>(this.OnPlayerCommand, true, threadTarget: ThreadTarget.PublisherThread);

            var allCommands = this.LoadCommands();

            this.Commands = allCommands
                            .ToDictionary(
                                          x => x.Key,
                                          x => (IImmutableDictionary<string, ICommand>)x.Value.ToImmutableDictionary())
                            .ToImmutableDictionary();

            this.NonAliasCommands = allCommands
                .ToDictionary(
                              x => x.Key,
                              x => (IImmutableDictionary<string, ICommand>)x.Value
                                                                            .Where(y => y.Value.AliasNames.Contains(y.Key) == false)
                                                                            .ToImmutableDictionary())
                .ToImmutableDictionary();

            foreach (var commandGroup in this.NonAliasCommands)
            {
                foreach (var command in commandGroup.Value)
                {
                    this.logger.LogTrace($"Command found: {command.Value.HelpSignature} {(command.Value.AliasNames.Length > 0 ? $"(+ {command.Value.AliasNames.Length} aliases)" : string.Empty)}");
                }
            }

            this.logger.LogInformation($"{this.Commands.Sum(x => x.Value.Count)} commands have been loaded");
        }

        internal void OnPlayerCommand(PlayerCommandEvent eventdata)
        {
            Guard.Argument(eventdata, nameof(eventdata)).NotNull();

            eventdata.Cancelled = true;

            this.ExecuteCommand(eventdata);
        }

        internal async void ExecuteCommand(PlayerCommandEvent eventdata)
        {
            var player = eventdata.Player;
            ICommand? command = null;

            try
            {
                // Unable to find specific command, return list of possible commands.
                if (this.TryGetCommandFromArgumentText(
                                                       eventdata.CommandText.Substring(1),
                                                       true,
                                                       out var potentialCommands,
                                                       out var groupName,
                                                       out var remainingArgumentText) == false)
                {
                    var accessibleCommands = await this.FilterCommandWithoutPermissionAsync(player, potentialCommands).ConfigureAwait(false);

                    this.eventAggregator.Publish(
                                                 new UnknownCommandEvent(
                                                                         player,
                                                                         eventdata.CommandText,
                                                                         accessibleCommands.ToImmutableDictionary(),
                                                                         groupName,
                                                                         remainingArgumentText));

                    return;
                }

                command = potentialCommands.Values.First();

                // Do not try to map arguments if the player can not execute the command at all.
                if (await command.CanExecuteCommandAsync(player).ConfigureAwait(false) == false)
                {
                    this.eventAggregator.Publish(
                                                 new CommandExecutedEvent(
                                                                          player,
                                                                          CommandResult.Failed(CommandExecutionStatus.NoPermission),
                                                                          null,
                                                                          command));

                    return;
                }

                // Apply current player to context of mapping for certain type conversions.
                void ApplyContext(IMappingOperationOptions options)
                {
                    options.Items[CommandConstants.CommandExecutorKey] = player;
                }

                string[] argumentParts =
                    this.SplitArgumentStringToArgumentList(remainingArgumentText, command.Parameters.Count - 1);
                object[] argumentValues = this.MapArgumentListToTypes(
                                                                      argumentParts,
                                                                      command.Parameters.Skip(1).Select(x => x.Type)
                                                                             .ToList(),
                                                                      ApplyContext);

                var result = await command.TryExecuteAsync(player, argumentValues, true).ConfigureAwait(false);

                this.eventAggregator.Publish(new CommandExecutedEvent(player, result, null, command));
            }
            catch (CommandArgumentMapException e) when (command != null)
            {
                var parameter = command.Parameters[e.ParameterIndex + 1];

                string? mappingMessage = null;
                if (e.InnerException != null && e.InnerException.InnerException != null)
                {
                    mappingMessage = e.InnerException.InnerException.Message;
                }

                this.eventAggregator.Publish(
                                             new CommandExecutedEvent(
                                                                      player,
                                                                      CommandResult.Failed(CommandExecutionStatus.ArgumentTypeMismatch, mappingMessage ?? string.Empty),
                                                                      parameter.Name,
                                                                      command));
            }
            catch (Exception e)
            {
                this.logger.LogWarning(e, $"Command failed to execute. ({player}: \"{eventdata.CommandText}\".");

                this.eventAggregator.Publish(
                                             new CommandExecutedEvent(
                                                                      player,
                                                                      CommandResult.Failed(CommandExecutionStatus.Exception, e.Message),
                                                                      null,
                                                                      command));
            }
        }

        internal bool TryGetCommandFromArgumentText(
            string argumentText,
            bool skipAlias,
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

                    IEnumerable<KeyValuePair<string, ICommand>> commands = groupCommands;
                    if (skipAlias)
                    {
                        // Add filter to remove commands aliases.
                        commands = groupCommands.Where(x => x.Value.AliasNames.Contains(x.Key) == false);
                    }

                    potentialCommands = commands.ToDictionary(x => $"{group} {x.Key}", x => x.Value);

                    return false;
                }

                // Or this command is inside ungrouped-group.
                if (this.Commands.TryGetValue(string.Empty, out groupCommands))
                {
                    groupName = string.Empty;

                    // Wanted command found, return.
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

            // Entered more parts, search directly for two parts.
            if (argumentParts.Length >= 2)
            {
                // Command has been found in group.
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

                // Command has been found in nongrouped-group.
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

            return argumentString.Split(new[] { ' ', }, argumentAmount);
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
                try
                {
                    result[i] = this.mapper.Map(arguments[i], typeof(string), types[i], contextApplicator);
                }
                catch (AutoMapperMappingException e)
                {
                    throw new CommandArgumentMapException(i, e);
                }
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
                    var commandName = command.Name.Trim();

                    if (result.TryGetValue(groupName, out var groupCommands) == false)
                    {
                        // Commandgroup does not exist, create.
                        result[groupName] = new Dictionary<string, ICommand>
                        {
                            { commandName, command },
                        };

                        groupCommands = result[groupName];
                    }
                    else
                    {
                        // Commandgroup exists, add to group
                        if (groupCommands.TryGetValue(commandName, out _))
                        {
                            throw new DuplicateCommandException(commandName, groupName);
                        }

                        groupCommands[commandName] = command;
                    }

                    // Add all aliasnames to group.
                    foreach (var aliasName in command.AliasNames)
                    {
                        if (groupCommands.TryGetValue(aliasName, out _))
                        {
                            throw new DuplicateCommandException(aliasName, groupName);
                        }

                        groupCommands[aliasName] = command;
                    }
                }
            }

            return result;
        }

        private async Task<IEnumerable<KeyValuePair<string, ICommand>>> FilterCommandWithoutPermissionAsync(
            IPlayer player,
            IEnumerable<KeyValuePair<string, ICommand>> dictionary)
        {
            var tasks = dictionary
                .ToDictionary(
                              x => x,
                              x => x.Value.CanExecuteCommandAsync(player));

            await Task.WhenAll(tasks.Values).ConfigureAwait(false);

            return tasks
                   .Where(x => x.Value.Result)
                   .Select(x => x.Key);
        }
    }
}
