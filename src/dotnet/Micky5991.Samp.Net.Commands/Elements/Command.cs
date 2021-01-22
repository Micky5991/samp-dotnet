using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dawn;
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
        /// <param name="name">Required name of this command.</param>
        /// <param name="aliasNames">Available alias names of this command.</param>
        /// <param name="group">Optional group name of this command.</param>
        /// <param name="parameters">List of parameters of this command.</param>
        /// <exception cref="ArgumentNullException"><paramref name="name"/>, <paramref name="aliasNames"/> or <paramref name="parameters"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="name"/> or <paramref name="group"/> is empty or whitespace, <paramref name="parameters"/> or <paramref name="aliasNames"/>contains null.</exception>
        protected Command(string name, string[] aliasNames, string? group, IReadOnlyList<ParameterDefinition> parameters)
        {
            Guard.Argument(name, nameof(name)).NotNull().NotWhiteSpace();
            Guard.Argument(aliasNames, nameof(aliasNames)).NotNull().DoesNotContainNull();
            Guard.Argument(group, nameof(group)).NotWhiteSpace();
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

            this.Name = name;
            this.AliasNames = aliasNames;
            this.Group = group;
            this.Parameters = parameters;

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

        /// <summary>
        /// Gets the minimal required argument amount for this command.
        /// </summary>
        protected int MinimalArgumentAmount { get; }

        /// <inheritdoc />
        public abstract CommandExecutionStatus TryExecute(IPlayer player, object[] arguments, out string? errorMessage);

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
