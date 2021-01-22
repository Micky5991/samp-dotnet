using Micky5991.Samp.Net.Commands.Attributes;
using Micky5991.Samp.Net.Commands.Interfaces;
using Micky5991.Samp.Net.Framework.Interfaces.Entities;

namespace Micky5991.Samp.Net.Commands.Tests.Fakes.CommandHandlers
{
    public class MultipleCommandsHandler : ICommandHandler
    {
        [Command("grouped", "command1")]
        public void Command1(IPlayer player)
        {
            // Empty
        }

        [Command("command2")]
        public void Command2(IPlayer player)
        {
            // Empty
        }
    }
}
