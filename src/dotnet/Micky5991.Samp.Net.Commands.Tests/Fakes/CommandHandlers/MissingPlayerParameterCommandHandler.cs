using Micky5991.Samp.Net.Commands.Attributes;
using Micky5991.Samp.Net.Commands.Interfaces;

namespace Micky5991.Samp.Net.Commands.Tests.Fakes.CommandHandlers
{
    public class MissingPlayerParameterCommandHandler : ICommandHandler
    {
        [Command("test")]
        public void Test()
        {

        }
    }
}
