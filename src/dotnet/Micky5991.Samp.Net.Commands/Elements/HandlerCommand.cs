using System;
using System.Collections.Generic;
using Dawn;
using JetBrains.Annotations;
using Micky5991.Samp.Net.Commands.Interfaces;
using Micky5991.Samp.Net.Framework.Interfaces.Entities;
using Microsoft.Extensions.Logging;

namespace Micky5991.Samp.Net.Commands.Elements
{
    /// <summary>
    /// Implementation that executes commands contained in a <see cref="ICommandHandler"/> implementation.
    /// </summary>
    public class HandlerCommand : Command
    {
        private readonly ILogger<HandlerCommand> logger;

        private readonly ICommandHandler commandHandler;

        private readonly Func<object[], object> executor;

        /// <summary>
        /// Initializes a new instance of the <see cref="HandlerCommand"/> class.
        /// </summary>
        /// <param name="logger">Logger for this command instance.</param>
        /// <param name="group">Optional group of this command.</param>
        /// <param name="name">Name of the command.</param>
        /// <param name="aliasNames">Alias names which are also available.</param>
        /// <param name="description">Description of this command.</param>
        /// <param name="parameters">Parameter information about this command.</param>
        /// <param name="commandHandler">Target instance of this command.</param>
        /// <param name="executor">Executor action to trigger the command.</param>
        public HandlerCommand(
            ILogger<HandlerCommand> logger,
            string? @group,
            string name,
            string[] aliasNames,
            string? description,
            IReadOnlyList<ParameterDefinition> parameters,
            ICommandHandler commandHandler,
            Func<object[], object> executor)
            : base(@group, name, aliasNames, description, parameters)
        {
            Guard.Argument(logger, nameof(logger)).NotNull();
            Guard.Argument(executor, nameof(executor)).NotNull();
            Guard.Argument(commandHandler, nameof(commandHandler)).NotNull();

            this.logger = logger;
            this.commandHandler = commandHandler;
            this.executor = executor;
        }

        /// <inheritdoc />
        public override CommandExecutionStatus TryExecute(IPlayer player, object[] arguments, out string? errorMessage)
        {
            Guard.Argument(player, nameof(player)).NotNull();
            Guard.Argument(arguments, nameof(arguments)).NotNull();

            errorMessage = string.Empty;
            if (arguments.Length < this.MinimalArgumentAmount - 1)
            {
                return CommandExecutionStatus.MissingArgument;
            }

            if (arguments.Length > this.Parameters.Count)
            {
                return CommandExecutionStatus.TooManyArguments;
            }

            var extendedArguments = new object[this.Parameters.Count];
            extendedArguments[0] = player;

            Array.Copy(arguments, 0, extendedArguments, 1, arguments.Length);

            this.FillMissingArguments(extendedArguments, arguments.Length + 1);

            if (this.ValidateArgumentTypes(extendedArguments) == false)
            {
                return CommandExecutionStatus.ArgumentTypeMismatch;
            }

            try
            {
                this.executor(extendedArguments);
            }
            catch (Exception e)
            {
                errorMessage = e.Message;

                return CommandExecutionStatus.Exception;
            }

            return CommandExecutionStatus.Ok;
        }
    }
}
