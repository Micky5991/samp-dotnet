using System;
using System.Collections.Generic;
using Micky5991.Samp.Net.Commands.Elements;
using Micky5991.Samp.Net.Framework.Interfaces.Entities;

namespace Micky5991.Samp.Net.Commands.Interfaces
{
    /// <summary>
    /// Command definition with a generic handler inside.
    /// </summary>
    public interface ICommand
    {
        /// <summary>
        /// Gets the unique group name of this command. Can be omitted.
        /// </summary>
        public string? Group { get; }

        /// <summary>
        /// Gets the unique name of this command. It has to be unique inside the given command group.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the list of <see cref="ParameterDefinition"/> of this command.
        /// </summary>
        public IReadOnlyList<ParameterDefinition> Parameters { get; }

        /// <summary>
        /// Executes the command with the specific sender and arguments.
        /// </summary>
        /// <param name="player">Sender that executed the command.</param>
        /// <param name="arguments">Already converted arguments to pass to the handler.</param>
        /// <param name="errorMessage">Message that should be returned, when the executor was unable to call the command.</param>
        /// <returns>true if the command was executed successfully, false otherwise.</returns>
        bool TryExecute(IPlayer player, IList<object> arguments, out string? errorMessage);
    }
}
