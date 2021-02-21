using System;
using System.Drawing;
using System.Net;
using System.Numerics;
using System.Security.Claims;
using System.Threading.Tasks;
using Dawn;
using Micky5991.Samp.Net.Core.Natives.Players;
using Micky5991.Samp.Net.Core.Natives.Samp;
using Micky5991.Samp.Net.Framework.Data;
using Micky5991.Samp.Net.Framework.Extensions;
using Micky5991.Samp.Net.Framework.Interfaces.Entities;
using Micky5991.Samp.Net.Framework.Interfaces.Entities.Pools;

namespace Micky5991.Samp.Net.Framework.Elements.Entities
{
    /// <inheritdoc cref="IPlayer"/>
    public partial class Player : Entity, IPlayer
    {
        private readonly IPlayerPool.RemoveEntityDelegate entityRemoval;

        private readonly ISampNatives sampNatives;

        private readonly IPlayersNatives playersNatives;

        /// <summary>
        /// Initializes a new instance of the <see cref="Player"/> class.
        /// </summary>
        /// <param name="id">The id of this player.</param>
        /// <param name="entityRemoval">Pool removal delgate.</param>
        /// <param name="sampNatives">General samp natives needed for this entity.</param>
        /// <param name="playersNatives">Natives needed for this entity.</param>
        public Player(
            int id,
            IPlayerPool.RemoveEntityDelegate entityRemoval,
            ISampNatives sampNatives,
            IPlayersNatives playersNatives)
            : base(id)
        {
            this.entityRemoval = entityRemoval;
            this.sampNatives = sampNatives;
            this.playersNatives = playersNatives;

            this.Principal = new ClaimsPrincipal();
        }

        /// <inheritdoc />
        public Vector3 Position
        {
            get
            {
                Guard.Disposal(this.Disposed);

                this.playersNatives.GetPlayerPos(this.Id, out var x, out var y, out var z);

                return new Vector3(x, y, z);
            }

            set
            {
                Guard.Disposal(this.Disposed);

                this.playersNatives.SetPlayerPos(this.Id, value.X, value.Y, value.Z);
            }
        }

        /// <inheritdoc />
        public float Rotation
        {
            get
            {
                Guard.Disposal(this.Disposed);

                this.playersNatives.GetPlayerFacingAngle(this.Id, out var angle);

                return angle;
            }

            set
            {
                Guard.Disposal(this.Disposed);

                this.playersNatives.SetPlayerFacingAngle(this.Id, value);
            }
        }

        /// <inheritdoc />
        public ClaimsPrincipal Principal { get; }

        /// <inheritdoc />
        public string Name
        {
            get
            {
                Guard.Disposal(this.Disposed);

                this.playersNatives.GetPlayerName(this.Id, out var name, SampConstants.MaxPlayerName);

                return name;
            }

            set
            {
                Guard.Disposal(this.Disposed);
                Guard.Argument(value, nameof(value)).NotWhiteSpace().LengthInRange(3, SampConstants.MaxPlayerName);

                this.playersNatives.SetPlayerName(this.Id, value);
            }
        }

        /// <inheritdoc />
        public IPAddress Ip
        {
            get
            {
                Guard.Disposal(this.Disposed);

                this.playersNatives.GetPlayerIp(this.Id, out var ip, 45);

                return IPAddress.Parse(ip);
            }
        }

        /// <inheritdoc />
        public int Ping
        {
            get
            {
                Guard.Disposal(this.Disposed);

                return this.playersNatives.GetPlayerPing(this.Id);
            }
        }

        /// <inheritdoc />
        public Vector3 Velocity
        {
            get
            {
                Guard.Disposal(this.Disposed);

                this.playersNatives.GetPlayerVelocity(this.Id, out var x, out var y, out var z);

                return new Vector3(x, y, z);
            }

            set
            {
                Guard.Disposal(this.Disposed);

                this.playersNatives.SetPlayerVelocity(this.Id, value.X, value.Y, value.Z);
            }
        }

        /// <inheritdoc />
        public int VirtualWorld
        {
            get
            {
                Guard.Disposal(this.Disposed);

                return this.playersNatives.GetPlayerVirtualWorld(this.Id);
            }

            set
            {
                Guard.Disposal(this.Disposed);

                this.playersNatives.SetPlayerVirtualWorld(this.Id, value);
            }
        }

        /// <inheritdoc />
        public int Money
        {
            get
            {
                Guard.Disposal(this.Disposed);

                return this.playersNatives.GetPlayerMoney(this.Id);
            }

            set
            {
                Guard.Disposal(this.Disposed);

                if (value == 0)
                {
                    this.playersNatives.ResetPlayerMoney(this.Id);

                    return;
                }

                this.playersNatives.GivePlayerMoney(this.Id, value - this.Money);
            }
        }

        /// <inheritdoc />
        public Color Color
        {
            get
            {
                Guard.Disposal(this.Disposed);

                var color = this.playersNatives.GetPlayerColor(this.Id);

                return Color.FromArgb(color);
            }

            set
            {
                Guard.Disposal(this.Disposed);

                this.SetColor(value);
            }
        }

        /// <inheritdoc />
        public float Health
        {
            get
            {
                Guard.Disposal(this.Disposed);

                this.playersNatives.GetPlayerHealth(this.Id, out var health);

                return health;
            }

            set
            {
                Guard.Disposal(this.Disposed);

                this.playersNatives.SetPlayerHealth(this.Id, value);
            }
        }

        /// <inheritdoc />
        public float Armor
        {
            get
            {
                Guard.Disposal(this.Disposed);

                this.playersNatives.GetPlayerArmour(this.Id, out var armor);

                return armor;
            }

            set
            {
                Guard.Disposal(this.Disposed);

                this.playersNatives.SetPlayerArmour(this.Id, value);
            }
        }

        /// <inheritdoc />
        public int Interior
        {
            get
            {
                Guard.Disposal(this.Disposed);

                return this.playersNatives.GetPlayerInterior(this.Id);
            }

            set
            {
                Guard.Disposal(this.Disposed);

                this.playersNatives.SetPlayerInterior(this.Id, value);
            }
        }

        /// <inheritdoc />
        public int? TargetPlayer
        {
            get
            {
                Guard.Disposal(this.Disposed);

                var target = this.playersNatives.GetPlayerTargetPlayer(this.Id);
                if (target == SampConstants.InvalidPlayerId)
                {
                    return null;
                }

                return target;
            }
        }

        /// <inheritdoc />
        public int? TargetActor
        {
            get
            {
                Guard.Disposal(this.Disposed);

                var target = this.playersNatives.GetPlayerTargetActor(this.Id);
                if (target == SampConstants.InvalidActorId)
                {
                    return null;
                }

                return target;
            }
        }

        /// <inheritdoc />
        public int Team
        {
            get
            {
                Guard.Disposal(this.Disposed);

                return this.playersNatives.GetPlayerTeam(this.Id);
            }

            set
            {
                Guard.Disposal(this.Disposed);

                this.playersNatives.SetPlayerTeam(this.Id, value);
            }
        }

        /// <inheritdoc />
        public int Score
        {
            get
            {
                Guard.Disposal(this.Disposed);

                return this.playersNatives.GetPlayerScore(this.Id);
            }

            set
            {
                Guard.Disposal(this.Disposed);

                this.playersNatives.SetPlayerScore(this.Id, value);
            }
        }

        /// <inheritdoc />
        public int DrunkLevel
        {
            get
            {
                Guard.Disposal(this.Disposed);

                return this.playersNatives.GetPlayerDrunkLevel(this.Id);
            }

            set
            {
                Guard.Disposal(this.Disposed);

                this.playersNatives.SetPlayerDrunkLevel(this.Id, value);
            }
        }

        /// <inheritdoc />
        public int Skin
        {
            get
            {
                Guard.Disposal(this.Disposed);

                return this.playersNatives.GetPlayerSkin(this.Id);
            }

            set
            {
                Guard.Disposal(this.Disposed);

                this.playersNatives.SetPlayerSkin(this.Id, value);
            }
        }

        /// <inheritdoc />
        public PlayerState State
        {
            get
            {
                Guard.Disposal(this.Disposed);

                return (PlayerState)this.playersNatives.GetPlayerState(this.Id);
            }
        }

        /// <inheritdoc />
        public TimeData Time
        {
            get
            {
                Guard.Disposal(this.Disposed);

                this.playersNatives.GetPlayerTime(this.Id, out var hours, out var minutes);

                return new TimeData(hours, minutes);
            }

            set
            {
                Guard.Disposal(this.Disposed);

                this.playersNatives.SetPlayerTime(this.Id, value.Hours, value.Minutes);
            }
        }

        /// <inheritdoc />
        public int WantedLevel
        {
            get
            {
                Guard.Disposal(this.Disposed);

                return this.playersNatives.GetPlayerWantedLevel(this.Id);
            }

            set
            {
                Guard.Disposal(this.Disposed);

                this.playersNatives.SetPlayerWantedLevel(this.Id, value);
            }
        }

        /// <inheritdoc />
        public void SendMessage(Color color, string message)
        {
            Guard.Argument(message).NotNull();
            Guard.Disposal(this.Disposed);

            this.sampNatives.SendClientMessage(this.Id, color.ToRgba(), message);
        }

        /// <inheritdoc />
        public void SetSpawnInfo(int team, int skin, Vector3 position, float rotation, params WeaponData[] weapons)
        {
            Guard.Disposal(this.Disposed);
            Guard.Argument(weapons, nameof(weapons)).NotNull().MaxCount(3);

            Array.Resize(ref weapons, 3);

            this.playersNatives.SetSpawnInfo(
                                             this.Id,
                                             team,
                                             skin,
                                             position.X,
                                             position.Y,
                                             position.Z,
                                             rotation,
                                             (int)weapons[0].Model,
                                             weapons[0].Ammo,
                                             (int)weapons[1].Model,
                                             weapons[1].Ammo,
                                             (int)weapons[2].Model,
                                             weapons[2].Ammo);
        }

        /// <inheritdoc />
        public async void Spawn()
        {
            Guard.Disposal(this.Disposed);

            await Task.Delay(1); // Needs to run in next tick.

            this.playersNatives.SpawnPlayer(this.Id);
        }

        /// <inheritdoc />
        public void ToggleClock(bool visible)
        {
            Guard.Disposal(this.Disposed);

            this.playersNatives.TogglePlayerClock(this.Id, visible);
        }

        /// <inheritdoc />
        public void ForceClassSelection()
        {
            Guard.Disposal(this.Disposed);

            this.playersNatives.ForceClassSelection(this.Id);
        }

        /// <inheritdoc />
        public void PlayCrimeReport(IPlayer suspect, int crime)
        {
            Guard.Disposal(this.Disposed);
            Guard.Argument(suspect, nameof(suspect)).NotNull();
            Guard.Disposal(suspect.Disposed, nameof(suspect));

            this.playersNatives.PlayCrimeReportForPlayer(this.Id, suspect.Id, crime);
        }

        /// <inheritdoc />
        public void PlayAudioStream(string url)
        {
            Guard.Disposal(this.Disposed);
            Guard.Argument(url, nameof(url)).NotNull().NotWhiteSpace();

            this.playersNatives.PlayAudioStreamForPlayer(this.Id, url, 0, 0, 0, 0, false);
        }

        /// <inheritdoc />
        public void PlayAudioStream(string url, Vector3 position, float distance = 50)
        {
            Guard.Disposal(this.Disposed);
            Guard.Argument(url, nameof(url)).NotNull().NotWhiteSpace();
            Guard.Argument(distance, nameof(distance)).NotNegative();

            this.playersNatives.PlayAudioStreamForPlayer(
                                                         this.Id,
                                                         url,
                                                         position.X,
                                                         position.Y,
                                                         position.Z,
                                                         distance,
                                                         true);
        }

        /// <inheritdoc />
        public void StopAudioStream()
        {
            Guard.Disposal(this.Disposed);

            this.playersNatives.StopAudioStreamForPlayer(this.Id);
        }

        /// <inheritdoc />
        public void SetShopName(string shopname)
        {
            Guard.Disposal(this.Disposed);
            Guard.Argument(shopname, nameof(shopname)).NotNull();

            this.playersNatives.SetPlayerShopName(this.Id, shopname);
        }

        /// <inheritdoc />
        public int? GetSurfingObjectId()
        {
            Guard.Disposal(this.Disposed);

            var objectId = this.playersNatives.GetPlayerSurfingObjectID(this.Id);

            if (objectId == SampConstants.InvalidObjectId)
            {
                return null;
            }

            return objectId;
        }

        /// <inheritdoc />
        public void RemoveBuildingForPlayer(int model, Vector3 position, float radius)
        {
            Guard.Disposal(this.Disposed);
            Guard.Argument(radius, nameof(radius)).NotNegative();

            this.playersNatives.RemoveBuildingForPlayer(this.Id, model, position.X, position.Y, position.Z, radius);
        }

        /// <inheritdoc />
        public void SetChatBubble(string text, Color color, float drawDistance, TimeSpan expireTime)
        {
            Guard.Argument(text, nameof(text)).NotNull().MaxLength(PlayersConstants.MaxChatbubbleLength);
            Guard.Argument(drawDistance, nameof(drawDistance)).NotNegative();
            Guard.Disposal(this.Disposed);

            this.playersNatives.SetPlayerChatBubble(
                                                    this.Id,
                                                    text,
                                                    color.ToRgba(),
                                                    drawDistance,
                                                    (int)expireTime.TotalMilliseconds);
        }

        /// <inheritdoc />
        public void ToggleControllable(bool controllable)
        {
            Guard.Disposal(this.Disposed);

            this.playersNatives.TogglePlayerControllable(this.Id, controllable);
        }

        /// <inheritdoc />
        public void PlaySound(int sound, Vector3 position)
        {
            Guard.Disposal(this.Disposed);

            this.playersNatives.PlayerPlaySound(this.Id, sound, position.X, position.Y, position.Z);
        }

        /// <inheritdoc />
        public void PlaySound(int sound)
        {
            Guard.Disposal(this.Disposed);

            this.playersNatives.PlayerPlaySound(this.Id, sound, 0, 0, 0);
        }

        /// <inheritdoc />
        public void ShowDialog(
            int dialogid,
            DialogStyle style,
            string caption,
            string info,
            string buttonLeft,
            string buttonRight = "")
        {
            Guard.Argument(dialogid, nameof(dialogid)).NotNegative();
            Guard.Argument(caption, nameof(caption)).NotNull();
            Guard.Argument(info, nameof(info)).NotNull();
            Guard.Argument(buttonLeft, nameof(buttonLeft)).NotNull();
            Guard.Argument(buttonRight, nameof(buttonRight)).NotNull();
            Guard.Disposal(this.Disposed);

            this.sampNatives.ShowPlayerDialog(this.Id, dialogid, (int)style, caption, info, buttonLeft, buttonRight);
        }

        /// <inheritdoc />
        public void HideDialogs()
        {
            Guard.Disposal(this.Disposed);

            this.sampNatives.ShowPlayerDialog(this.Id, -1, 0, " ", " ", " ", " ");
        }

        /// <inheritdoc />
        public bool IsRconAdmin()
        {
            Guard.Disposal(this.Disposed);

            return this.sampNatives.IsPlayerAdmin(this.Id);
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return $"<{this.GetType()} ({this.Id}) {this.Name}>";
        }

        /// <inheritdoc />
        protected override void DisposeEntity()
        {
            this.entityRemoval(this);
        }

        private async void SetColor(Color color)
        {
            await Task.Delay(1); // Needs to run 1 tick after OnPlayerConnect.

            this.playersNatives.SetPlayerColor(this.Id, color.ToArgb());
        }
    }
}
