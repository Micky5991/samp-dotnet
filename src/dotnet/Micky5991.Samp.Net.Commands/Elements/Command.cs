using System;
using System.Collections.Generic;
using Micky5991.Samp.Net.Commands.Interfaces;
using Micky5991.Samp.Net.Framework.Interfaces.Entities;

namespace Micky5991.Samp.Net.Commands.Elements
{
    /// <inheritdoc />
    public class Command : ICommand
    {
        /// <inheritdoc />
        public string? Group { get; }

        /// <inheritdoc />
        public string Name { get; }

        /// <inheritdoc />
        public IReadOnlyList<ParameterDefinition> Parameters { get; }

        /// <inheritdoc />
        public bool TryExecute(IPlayer player, IList<object> arguments, out string? errorMessage)
        {
            throw new NotImplementedException();
        }
    }
}
