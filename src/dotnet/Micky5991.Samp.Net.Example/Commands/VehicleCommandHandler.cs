using System.Drawing;
using Micky5991.Samp.Net.Commands.Attributes;
using Micky5991.Samp.Net.Commands.Interfaces;
using Micky5991.Samp.Net.Framework.Interfaces.Entities;
using Micky5991.Samp.Net.Framework.Interfaces.Entities.Pools;
using Microsoft.AspNetCore.Authorization;
using Vehicle = Micky5991.Samp.Net.Core.Natives.Samp.Vehicle;

namespace Micky5991.Samp.Net.Example.Commands
{
    [Authorize(Policy = "VehicleCommands")]
    public class VehicleCommandHandler : ICommandHandler
    {
        private readonly IVehiclePool vehiclePool;

        public VehicleCommandHandler(IVehiclePool vehiclePool)
        {
            this.vehiclePool = vehiclePool;
        }

        [Command("veh", "spawn", Description = "Spawns a temporary vehicle on your location.")]
        [CommandAlias("s")]
        public void Test(IPlayer player, Vehicle model, int color1 = 0, int color2 = 0)
        {
            var vehicle = this.vehiclePool.CreateVehicle(
                                                         model,
                                                         player.Position,
                                                         player.Rotation,
                                                         color1,
                                                         color2);

            player.PutPlayerIntoVehicle(vehicle, 0);

            player.SendMessage(Color.DeepSkyBlue, $"You have been spawned into a {model}.");
        }

        [Command("veh", "repair", Description = "Repairs the vehicle you are currently in.")]
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

        [Command("veh", "destroy", Description = "Destroys the vehicle you are currently in.")]
        [CommandAlias("d")]
        public void Destroy(IPlayer player)
        {
            if (player.IsInAnyVehicle == false || player.VehicleId.HasValue == false ||
                this.vehiclePool.Entities.TryGetValue(player.VehicleId.Value, out var vehicle) == false)
            {
                player.SendMessage(Color.LightGray, "You are currently in no vehicle.");

                return;
            }

            vehicle.Destroy();

            player.SendMessage(Color.DeepSkyBlue, "Vehicle has been destroyed.");
        }
    }
}
