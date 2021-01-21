using Micky5991.Samp.Net.Commands.Attributes;
using Micky5991.Samp.Net.Commands.Interfaces;
using Micky5991.Samp.Net.Framework.Interfaces.Entities;

namespace Micky5991.Samp.Net.Commands.Tests.Fakes.CommandHandlers
{
    public class DuplicatedCommandNameCommandHandler : ICommandHandler
    {
        [Command("test")]
        public void Test(IPlayer player, string message)
        {

        }

        [Command("test")]
        public void Test(IPlayer player, int number)
        {

        }
    }
}
