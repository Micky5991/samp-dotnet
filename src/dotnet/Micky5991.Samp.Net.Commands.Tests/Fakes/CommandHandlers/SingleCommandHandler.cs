using Micky5991.Samp.Net.Commands.Attributes;
using Micky5991.Samp.Net.Commands.Interfaces;
using Micky5991.Samp.Net.Framework.Interfaces.Entities;

namespace Micky5991.Samp.Net.Commands.Tests.Fakes.CommandHandlers
{
    public class SingleCommandHandler : ICommandHandler
    {

        [Command("veh")]
        public void CreateVehicle(IPlayer player)
        {

        }

    }
}
