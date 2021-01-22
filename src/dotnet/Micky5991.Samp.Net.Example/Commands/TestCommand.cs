using System.Drawing;
using Micky5991.Samp.Net.Commands.Attributes;
using Micky5991.Samp.Net.Commands.Interfaces;
using Micky5991.Samp.Net.Core.Natives.Samp;
using Micky5991.Samp.Net.Framework.Interfaces.Entities;
using Micky5991.Samp.Net.Framework.Interfaces.Pools;

namespace Micky5991.Samp.Net.Example.Commands
{
    public class TestCommandHandler : ICommandHandler
    {
        private readonly IVehiclePool vehiclePool;

        public TestCommandHandler(IVehiclePool vehiclePool)
        {
            this.vehiclePool = vehiclePool;
        }

        [Command("spawn", "veh")]
        public void Test(IPlayer player)
        {
            var vehicle = this.vehiclePool.CreateVehicle(
                                                         Vehicle.Bullet,
                                                         player.Position,
                                                         0,
                                                         0,
                                                         150);

            player.PutPlayerIntoVehicle(vehicle, 0);

            player.SendMessage(Color.LawnGreen, "You have been spawned into a bullet.");
        }

    }
}
