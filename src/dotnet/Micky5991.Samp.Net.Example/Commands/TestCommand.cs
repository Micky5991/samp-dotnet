using System.Drawing;
using System.Linq;
using Micky5991.Samp.Net.Commands.Attributes;
using Micky5991.Samp.Net.Commands.Interfaces;
using Micky5991.Samp.Net.Core.Natives.Samp;
using Micky5991.Samp.Net.Framework.Extensions;
using Micky5991.Samp.Net.Framework.Interfaces.Entities;
using Micky5991.Samp.Net.Framework.Interfaces.Entities.Pools;
using Microsoft.AspNetCore.Authorization;

namespace Micky5991.Samp.Net.Example.Commands
{
    public class TestCommandHandler : ICommandHandler
    {
        private readonly IVehiclePool vehiclePool;

        public TestCommandHandler(IVehiclePool vehiclePool)
        {
            this.vehiclePool = vehiclePool;
        }

        [Command("claims", Description = "Lists all claims of the given user")]
        public void Claims(IPlayer player, IPlayer target)
        {
            player.SendMessage(Color.DeepSkyBlue, "");
            player.SendMessage(Color.DeepSkyBlue, $"| * Current claims of {target.Name}:");
            player.SendMessage(Color.DeepSkyBlue, "| ---------------------------------------------------");

            foreach (var group in target.Principal.Claims.GroupBy(x => x.Subject?.AuthenticationType))
            {
                player.SendMessage(Color.DeepSkyBlue, $"| {Color.White.Embed()}Authentication type \"{Color.DarkGray.Embed()}{group.Key}{Color.DeepSkyBlue.Embed()}\"");

                foreach (var claim in group)
                {
                    player.SendMessage(Color.DeepSkyBlue, $"| - {Color.Lime.Embed()}{claim.Type}{Color.DeepSkyBlue.Embed()}: {Color.White.Embed()}{claim.Value}");
                }

                player.SendMessage(Color.DeepSkyBlue, "| ---------------------------------------------------");
            }
        }

        [Authorize(Policy = "VehicleCommands")]
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

            player.SendMessage(Color.DeepSkyBlue, "You have been spawned into a bullet.");
        }

        [Authorize(Policy = "VehicleCommands")]
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

        [Authorize]
        [Command("player", "weapon")]
        public void GiveWeapon(IPlayer player, IPlayer target, Weapon weapon, int ammo = 100)
        {
            target.GivePlayerWeapon(weapon, ammo);

            player.SendMessage(Color.DeepSkyBlue, $"Player received the weapon {weapon} with {ammo} ammo.");
        }

        [Command("player", "kill", Description = "Kills the given player")]
        public void KillPlayer(IPlayer player, IPlayer target)
        {
            target.Health = 0;

            player.SendMessage(Color.DeepSkyBlue, $"Player {target} has been killed.");
        }

    }
}
