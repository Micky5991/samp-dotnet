using System;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using Micky5991.Quests.Interfaces.Services;
using Micky5991.Samp.Net.Commands.Attributes;
using Micky5991.Samp.Net.Commands.Interfaces;
using Micky5991.Samp.Net.Core.Natives.Samp;
using Micky5991.Samp.Net.Example.Quests;
using Micky5991.Samp.Net.Example.Services;
using Micky5991.Samp.Net.Framework.Constants;
using Micky5991.Samp.Net.Framework.Elements.TextDraws;
using Micky5991.Samp.Net.Framework.Enums;
using Micky5991.Samp.Net.Framework.Extensions;
using Micky5991.Samp.Net.Framework.Interfaces.Entities;
using Micky5991.Samp.Net.Framework.Interfaces.Entities.Pools;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;

namespace Micky5991.Samp.Net.Example.Commands
{
    public class TestCommandHandler : ICommandHandler
    {
        private readonly IVehiclePool vehiclePool;

        private readonly IQuestFactory questFactory;

        private readonly PlayerQuestsManager playerQuestsManager;

        private readonly ILogger<TestCommandHandler> logger;

        public TestCommandHandler(IVehiclePool vehiclePool, IQuestFactory questFactory, PlayerQuestsManager playerQuestsManager, ILogger<TestCommandHandler> logger)
        {
            this.vehiclePool = vehiclePool;
            this.questFactory = questFactory;
            this.playerQuestsManager = playerQuestsManager;
            this.logger = logger;
        }

        [Command("claims", Description = "Lists all claims of the given user")]
        public void Claims(IPlayer player, IPlayer target)
        {
            player.SendMessage(Color.DeepSkyBlue, "");
            player.SendMessage(Color.DeepSkyBlue, $"| * Current claims of {target.Name}:");
            player.SendMessage(Color.DeepSkyBlue, "| ---------------------------------------------------");

            foreach (var group in target.Principal.Claims.GroupBy(x => x.Subject?.AuthenticationType))
            {
                player.SendMessage(Color.DeepSkyBlue, $"| {Color.White.Embed()}Authentication type \"{Color.DarkGray.Embed()}{group.Key}{Color.White.Embed()}\"");

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

            player.SendMessage(Color.DeepSkyBlue, $"You have been spawned into a {model}.");
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

        [Authorize(Policy = "VehicleCommands")]
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

        [Command("player", "quest", Description = "Gives the player the welcome quest")]
        public void GiveWelcomeQuest(IPlayer player, IPlayer target)
        {
            var quest = this.questFactory.BuildQuest<WelcomeQuest>();
            this.playerQuestsManager.GivePlayerQuest(player, quest);

            player.SendMessage(Color.DeepSkyBlue, $"Player {target} received the quest {quest.Title}.");
        }

        [Command("player", "qr", Description = "Refreshes the current player quests")]
        public void RefreshWelcomeQuest(IPlayer player, IPlayer target)
        {
            try
            {
                this.playerQuestsManager.RefreshQuests(player);

                player.SendMessage(Color.DeepSkyBlue, $"Quest display of {target} has been refreshed.");
            }
            catch (Exception e)
            {
                this.logger.LogError(e, "An error occured");
            }
        }

        [Authorize]
        [Command("player", "money", Description = "Adds or remove money from player")]
        public void KillPlayer(IPlayer player, IPlayer target, int money)
        {
            target.Money += money;

            player.SendMessage(Color.DeepSkyBlue, $"Player {target} has been given ${money}.");
        }

        [Command("t", "s")]
        public async Task ShowTextDraw(IPlayer player, IPlayer target)
        {
            try
            {
                var textDraw = new TextDraw(new Vector2(200, 200), "XD: " + (new Random()).Next(100, 999))
                {
                    TextColor = Color.Fuchsia,
                    ShadowSize = 0,
                    TextAlignment = TextAlignment.Centered,
                    TextFont = TextFont.BankGothic,
                    BoxColor = Color.Black.Transparentize(0.5f),
                    UseBox = true,
                    TextSize = new Vector2(100, 100),
                };

                target.ShowTextDraw(textDraw);

                var down = true;
                var right = true;
                while (true)
                {
                    var position = textDraw.Position;

                    if (down)
                    {
                        position.Y += 2;
                    }
                    else
                    {
                        position.Y -= 2;
                    }

                    if (right)
                    {
                        position.X += 2;
                    }
                    else
                    {
                        position.X -= 2;
                    }

                    if (position.Y is < 0 or > ScreenConstants.MaxHeight - 100)
                    {
                        down = down == false;
                    }

                    if (position.X is < 0 or > ScreenConstants.MaxWidth - 100)
                    {
                        right = right == false;
                    }

                    textDraw.Position = position;

                    await Task.Delay(50);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

        }

    }
}
