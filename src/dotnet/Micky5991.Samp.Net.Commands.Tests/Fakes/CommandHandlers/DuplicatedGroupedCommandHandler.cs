using Micky5991.Samp.Net.Commands.Attributes;
using Micky5991.Samp.Net.Commands.Interfaces;
using Micky5991.Samp.Net.Framework.Interfaces.Entities;

namespace Micky5991.Samp.Net.Commands.Tests.Fakes.CommandHandlers
{
    public class DuplicatedGroupedCommandHandler : ICommandHandler
    {
        [Command("verb", "group")]
        public void HandlerFirst(IPlayer player)
        {
            // Empty
        }
    }
}
