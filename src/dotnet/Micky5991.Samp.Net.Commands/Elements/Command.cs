using System;
using System.Collections.Generic;
using System.Linq;
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
        /// <param name="group">Optional group name of this command.</param>
        /// <param name="parameters">List of parameters of this command.</param>
        protected Command(string name, string? group, IReadOnlyList<ParameterDefinition> parameters)
        {
            Guard.Argument(name, nameof(name)).NotNull().NotWhiteSpace();
            Guard.Argument(group, nameof(group)).NotWhiteSpace();
            Guard.Argument(parameters, nameof(parameters)).NotEmpty().NotNull().DoesNotContainNull();

            if (parameters.Count >= 1 && parameters[0].Type != typeof(IPlayer))
            {
                throw new ArgumentException(
                                            "The first entry has to have the player as parameter.",
                                            nameof(parameters));
            }

            this.Name = name;
            this.Group = group;
            this.Parameters = parameters;

            this.MinimalArgumentAmount = this.Parameters.Count(x => x.HasDefault == false);
        }

        /// <inheritdoc />
        public string? Group { get; }

        /// <inheritdoc />
        public string Name { get; }

        /// <inheritdoc />
        public IReadOnlyList<ParameterDefinition> Parameters { get; }

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
    }
}
