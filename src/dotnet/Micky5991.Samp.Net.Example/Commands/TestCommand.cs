using System.Drawing;
using Micky5991.Samp.Net.Commands.Attributes;
using Micky5991.Samp.Net.Commands.Interfaces;
using Micky5991.Samp.Net.Core.Natives.Samp;
using Micky5991.Samp.Net.Framework.Interfaces.Entities;
using Micky5991.Samp.Net.Framework.Interfaces.Entities.Pools;

namespace Micky5991.Samp.Net.Example.Commands
{
    public class TestCommandHandler : ICommandHandler
    {
        private readonly IVehiclePool vehiclePool;

        public TestCommandHandler(IVehiclePool vehiclePool)
        {
            this.vehiclePool = vehiclePool;
        }

        [Command("veh", "spawn", Description = "Spawns a temporary vehicle on your location.", Permission = "example.command.vehicle.spawn")]
        [CommandAlias("s")]
        public void Test(IPlayer player, Vehicle model)
        {
            var vehicle = this.vehiclePool.CreateVehicle(
                                                         model,
                                                         player.Position,
                                                         player.Rotation,
                                                         0,
                                                         150);

            player.PutPlayerIntoVehicle(vehicle, 0);

            player.SendMessage(Color.DeepSkyBlue, "You have been spawned into a bullet.");
        }

        [Command("veh", "repair", Description = "Repairs the vehicle you are currently in.", Permission = "example.command.vehicle.repair")]
        [CommandAlias("r")]
        public void Repair(IPlayer player)
        {
            if (player.IsInAnyVehicle == false || player.VehicleId.HasValue == false ||
                this.vehiclePool.Entities.TryGetValue(player.VehicleId.Value, out var vehicle) == false)
            {
                player.SendMessage(Color.LightGray, "You are currently in no vehicle.");

                return;
            }

            vehicle.Repair();

            player.SendMessage(Color.DeepSkyBlue, "Vehicle has been repaired.");
        }

    }
}
