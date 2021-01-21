using Micky5991.Samp.Net.Commands.Attributes;
using Micky5991.Samp.Net.Commands.Interfaces;
using Micky5991.Samp.Net.Framework.Interfaces.Entities;

namespace Micky5991.Samp.Net.Commands.Tests.Fakes.CommandHandlers
{
    public class ExecutionTesterCommandHandler : ICommandHandler
    {
        public object[] Arguments { get; private set; }

        [Command("test")]
        public void Command(IPlayer player, string argument, int number, bool isTrue = false)
        {
            this.Arguments = new object[]
            {
                player,
                argument,
                number,
                isTrue
            };
        }
    }
}
