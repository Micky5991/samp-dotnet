using System.Collections.Generic;

namespace Micky5991.Samp.Net.Commands.Interfaces
{
    /// <summary>
    /// Factory that builds <see cref="ICommand"/> instances.
    /// </summary>
    public interface ICommandFactory
    {
        ICollection<ICommand> BuildFromCommandHandler(ICommandHandler commandHandler);
    }
}
