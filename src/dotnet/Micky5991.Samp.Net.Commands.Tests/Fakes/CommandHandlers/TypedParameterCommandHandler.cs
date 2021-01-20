using Micky5991.Samp.Net.Commands.Attributes;
using Micky5991.Samp.Net.Commands.Interfaces;
using Micky5991.Samp.Net.Framework.Interfaces.Entities;

namespace Micky5991.Samp.Net.Commands.Tests.Fakes.CommandHandlers
{
    public class TypedParameterCommandHandler : ICommandHandler
    {
        [Command("veh")]
        public void Command(IPlayer player, string number, int type)
        {

        }
    }
}
