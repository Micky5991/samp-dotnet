using System.Collections.Generic;
using Micky5991.Samp.Net.Commands.Interfaces;

namespace Micky5991.Samp.Net.Commands.Tests.Fakes
{
    public class FakeCommandFactory : ICommandFactory
    {
        public ICollection<ICommand> BuildFromCommandHandler(ICommandHandler commandHandler)
        {
            throw new System.NotImplementedException();
        }
    }
}
