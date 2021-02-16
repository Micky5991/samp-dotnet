using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dawn;
using Micky5991.Samp.Net.Commands.Attributes;
using Micky5991.Samp.Net.Commands.Data.Results;
using Micky5991.Samp.Net.Commands.Interfaces;
using Micky5991.Samp.Net.Framework.Interfaces.Entities;
using Micky5991.Samp.Net.Framework.Interfaces.Facades;
using Microsoft.AspNetCore.Authorization;

namespace Micky5991.Samp.Net.Commands.Elements
{
    /// <summary>
    /// Implementation that executes commands contained in a <see cref="ICommandHandler"/> implementation.
    /// </summary>
    public class HandlerCommand : Command
    {
        private readonly Func<object[], object> executor;

        /// <summary>
        /// Initializes a new instance of the <see cref="HandlerCommand"/> class.
        /// </summary>
        /// <param name="authorization">Service that determines if the user has access.</param>
        /// <param name="attribute">General attribute that describes this command.</param>
        /// <param name="authorizeAttributes">Attributes attached to this command.</param>
        /// <param name="aliasNames">Alias names which are also available.</param>
        /// <param name="parameters">Parameter information about this command.</param>
        /// <param name="executor">Executor action to trigger the command.</param>
        public HandlerCommand(
            IAuthorizationFacade authorization,
            CommandAttribute attribute,
            AuthorizeAttribute[] authorizeAttributes,
            string[] aliasNames,
            IReadOnlyList<ParameterDefinition> parameters,
            Func<object[], object> executor)
            : base(authorization, attribute, authorizeAttributes, aliasNames, parameters)
        {
            Guard.Argument(executor, nameof(executor)).NotNull();

            this.executor = executor;
        }

        /// <inheritdoc />
        public override async Task<CommandResult> TryExecuteAsync(
            IPlayer player,
            object[] arguments,
            bool skipPermissions)
        {
            Guard.Argument(player, nameof(player)).NotNull();
            Guard.Argument(arguments, nameof(arguments)).NotNull();

            if (skipPermissions == false && await this.CanExecuteCommandAsync(player) == false)
            {
                return CommandResult.Failed(CommandExecutionStatus.NoPermission);
            }

            if (arguments.Length < this.MinimalArgumentAmount - 1)
            {
                return CommandResult.Failed(CommandExecutionStatus.MissingArgument);
            }

            if (arguments.Length > this.Parameters.Count)
            {
                return CommandResult.Failed(CommandExecutionStatus.TooManyArguments);
            }

            var extendedArguments = new object[this.Parameters.Count];
            extendedArguments[0] = player;

            Array.Copy(arguments, 0, extendedArguments, 1, arguments.Length);

            this.FillMissingArguments(extendedArguments, arguments.Length + 1);

            if (this.ValidateArgumentTypes(extendedArguments) == false)
            {
                return CommandResult.Failed(CommandExecutionStatus.ArgumentTypeMismatch);
            }

            try
            {
                this.executor(extendedArguments);
            }
            catch (Exception e)
            {
                return CommandResult.Failed(CommandExecutionStatus.Exception, e.Message);
            }

            return CommandResult.Success();
        }
    }
}
