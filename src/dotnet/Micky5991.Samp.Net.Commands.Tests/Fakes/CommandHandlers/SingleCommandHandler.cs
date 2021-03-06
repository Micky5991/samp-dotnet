using JetBrains.Annotations;
using Micky5991.Samp.Net.Commands.Attributes;
using Micky5991.Samp.Net.Commands.Interfaces;
using Micky5991.Samp.Net.Framework.Interfaces.Entities;

namespace Micky5991.Samp.Net.Commands.Tests.Fakes.CommandHandlers
{
    public class SingleCommandHandler : ICommandHandler
    {
        public const string CommandName = "veh";
        [CanBeNull] public const string CommandGroup = null;

        [Command(CommandGroup, CommandName)]
        public void CreateVehicle(IPlayer player)
        {
        }

    }
}
