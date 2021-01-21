using System;
using System.Collections.Generic;

namespace Micky5991.Samp.Net.Commands.Interfaces
{
    /// <summary>
    /// Factory that builds <see cref="ICommand"/> instances.
    /// </summary>
    public interface ICommandFactory
    {
        /// <summary>
        /// Builds all command instances for the created command methods in this handler.
        /// </summary>
        /// <param name="commandHandler"><see cref="ICommandHandler"/> instances to search in.</param>
        /// <returns>List of <see cref="ICommandHandler"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="commandHandler"/> is null.</exception>
        ICollection<ICommand> BuildFromCommandHandler(ICommandHandler commandHandler);
    }
}
