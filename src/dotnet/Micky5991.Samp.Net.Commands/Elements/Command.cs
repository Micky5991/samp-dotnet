using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dawn;
using Micky5991.Samp.Net.Commands.Attributes;
using Micky5991.Samp.Net.Commands.Interfaces;
using Micky5991.Samp.Net.Framework.Interfaces.Entities;

namespace Micky5991.Samp.Net.Commands.Elements
{
    /// <inheritdoc />
    public abstract class Command : ICommand
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Command"/> class.
        /// </summary>
        /// <param name="attribute">Attribute that describes this command.</param>
        /// <param name="aliasNames">Available alias names of this command.</param>
        /// <param name="parameters">List of parameters of this command.</param>
        /// <exception cref="ArgumentNullException"><paramref name="attribute"/>, <paramref name="aliasNames"/> or <paramref name="parameters"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="parameters"/> or <paramref name="aliasNames"/>contains null.</exception>
        protected Command(
            CommandAttribute attribute,
            string[] aliasNames,
            IReadOnlyList<ParameterDefinition> parameters)
        {
            Guard.Argument(attribute, nameof(attribute)).NotNull();
            Guard.Argument(aliasNames, nameof(aliasNames)).NotNull().DoesNotContainNull();
            Guard.Argument(parameters, nameof(parameters)).NotEmpty().NotNull().DoesNotContainNull();
            Guard.Argument(parameters.Select(x => x.Name), nameof(parameters)).DoesNotContainDuplicate((_, _) => "All parameter names need to be unique.");

            if (parameters.Count >= 1 && parameters[0].Type != typeof(IPlayer))
            {
                throw new ArgumentException(
                                            "The first entry has to have the player as parameter.",
                                            nameof(parameters));
            }

            var lastDefault = false;
            foreach (var definition in parameters)
            {
                if (lastDefault && definition.HasDefault == false)
                {
                    throw new ArgumentException(
                                                $"There cannot be a parameter with no default value after a parameter with default value.",
                                                nameof(parameters));
                }

                lastDefault = definition.HasDefault;
            }

            this.Name = attribute.Name;
            this.AliasNames = aliasNames;
            this.Group = attribute.Group;
            this.Parameters = parameters;
            this.Description = attribute.Description ?? string.Empty;
            this.NeededPermission = attribute.Permission;

            this.MinimalArgumentAmount = this.Parameters.Count(x => x.HasDefault == false);
            this.HelpSignature = this.BuildHelpSignature();
        }

        /// <inheritdoc />
        public string? Group { get; }

        /// <inheritdoc />
        public string Name { get; }

        /// <inheritdoc />
        public string[] AliasNames { get; }

        /// <inheritdoc />
        public IReadOnlyList<ParameterDefinition> Parameters { get; }

        /// <inheritdoc />
        public string HelpSignature { get; }

        /// <inheritdoc />
        public string Description { get; }

        /// <inheritdoc />
        public string? NeededPermission { get; }

        /// <summary>
        /// Gets the minimal required argument amount for this command.
        /// </summary>
        protected int MinimalArgumentAmount { get; }

        /// <inheritdoc />
        public abstract CommandExecutionStatus TryExecute(IPlayer player, object[] arguments, out string? errorMessage);

        /// <inheritdoc />
        public virtual bool CanExecuteCommand(IPlayer player)
        {
            if (string.IsNullOrWhiteSpace(this.NeededPermission))
            {
                return true;
            }

            return player.HasPermission(this.NeededPermission!);
        }

        /// <summary>
        /// Validates if the passed arguments are compatible with all parameters.
        /// </summary>
        /// <param name="arguments">Argument values to check.</param>
        /// <returns>true if the values are compatible, false otherwise.</returns>
        protected bool ValidateArgumentTypes(object[] arguments)
        {
            for (var i = 0; i < arguments.Length && i < this.Parameters.Count; i++)
            {
                if (this.Parameters[i].Type.IsInstanceOfType(arguments[i]) == false)
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Fills the given array with default parameter values.
        /// </summary>
        /// <param name="arguments">Values to extend.</param>
        /// <param name="index">Start index to fill items from.</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/> is equal or higher than the length of <see cref="Parameters"/>.</exception>
        protected void FillMissingArguments(object[] arguments, int index)
        {
            Guard.Argument(index, nameof(index)).Max(this.Parameters.Count);

            if (index == this.Parameters.Count)
            {
                return;
            }

            for (var i = index; i < this.Parameters.Count; i++)
            {
                arguments[i] = this.Parameters[i].DefaultValue!;
            }
        }

        /// <summary>
        /// Generates a string that should be used when the user requests the command signature.
        /// </summary>
        /// <returns>Generated help signature.</returns>
        protected virtual string BuildHelpSignature()
        {
            var builder = new StringBuilder();

            if (this.Group == null)
            {
                builder.Append($"/{this.Name}");
            }
            else
            {
                builder.Append($"/{this.Group} {this.Name}");
            }

            foreach (var definition in this.Parameters.Skip(1))
            {
                if (definition.HasDefault == false)
                {
                    builder.Append($" [{definition.Name}]");
                }
                else
                {
                    builder.Append($" <{definition.Name}>");
                }
            }

            return builder.ToString();
        }
    }
}
