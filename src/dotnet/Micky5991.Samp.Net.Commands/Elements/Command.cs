using System;
using System.Collections.Generic;
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
            this.Name = name;
            this.Group = group;
            this.Parameters = parameters;
        }

        /// <inheritdoc />
        public string? Group { get; }

        /// <inheritdoc />
        public string Name { get; }

        /// <inheritdoc />
        public IReadOnlyList<ParameterDefinition> Parameters { get; }

        /// <inheritdoc />
        public abstract bool TryExecute(IPlayer player, IList<object> arguments, out string? errorMessage);
    }
}
