using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Micky5991.Samp.Net.Commands.Data.Results;
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
        /// Gets the current alias names of this command.
        /// </summary>
        public string[] AliasNames { get; }

        /// <summary>
        /// Gets the list of <see cref="ParameterDefinition"/> of this command.
        /// </summary>
        public IReadOnlyList<ParameterDefinition> Parameters { get; }

        /// <summary>
        /// Gets the signature that should be displayed if requested.
        /// </summary>
        public string HelpSignature { get; }

        /// <summary>
        /// Gets the help description for this command.
        /// </summary>
        public string Description { get; }

        /// <summary>
        /// Executes the command with the specific sender and arguments.
        /// </summary>
        /// <param name="player">Sender that executed the command.</param>
        /// <param name="arguments">Already converted arguments to pass to the handler.</param>
        /// <param name="ignorePermissions">true if permission check should be skipped, false otherwise.</param>
        /// <returns>Status of this call after the internal handler has been executed and message that should be returned, when the executor was unable to call the command..</returns>
        /// <exception cref="ArgumentNullException"><paramref name="player"/> or <paramref name="arguments"/> is null.</exception>
        Task<CommandResult> TryExecuteAsync(IPlayer player, object[] arguments, bool ignorePermissions = false);

        /// <summary>
        /// Checks if the given <paramref name="player"/> is able to execute this command.
        /// </summary>
        /// <param name="player">Player to check if the command can be executed.</param>
        /// <returns>true if the command can be executed, false otherwise.</returns>
        public Task<bool> CanExecuteCommandAsync(IPlayer player);
    }
}
