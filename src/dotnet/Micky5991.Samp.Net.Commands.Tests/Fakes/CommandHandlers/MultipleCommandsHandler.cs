using Micky5991.Samp.Net.Commands.Attributes;
using Micky5991.Samp.Net.Commands.Interfaces;

namespace Micky5991.Samp.Net.Commands.Tests.Fakes.CommandHandlers
{
    public class MultipleCommandsHandler : ICommandHandler
    {
        [Command("command1", "grouped")]
        public void Command1()
        {
            // Empty
        }

        [Command("command2")]
        public void Command2()
        {
            // Empty
        }
    }
}
