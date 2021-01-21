using Micky5991.Samp.Net.Commands.Attributes;
using Micky5991.Samp.Net.Commands.Interfaces;
using Micky5991.Samp.Net.Framework.Interfaces.Entities;

namespace Micky5991.Samp.Net.Commands.Tests.Fakes.CommandHandlers
{
    public class DuplicatedCommandNameButDifferentGroupNameCommandHandler : ICommandHandler
    {
        [Command("test", "groupA")]
        public void Test(IPlayer player)
        {
            // Empty
        }

        [Command("test", "groupB")]
        public void Test2(IPlayer player)
        {
            // Empty
        }
    }
}
