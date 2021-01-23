using System;
using System.Drawing;
using System.Net;
using System.Numerics;
using Dawn;
using Micky5991.Samp.Net.Core.Natives.Players;
using Micky5991.Samp.Net.Core.Natives.Samp;
using Micky5991.Samp.Net.Framework.Data;
using Micky5991.Samp.Net.Framework.Extensions;
using Micky5991.Samp.Net.Framework.Interfaces.Entities;
using Micky5991.Samp.Net.Framework.Interfaces.Pools;

namespace Micky5991.Samp.Net.Framework.Entities
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

                this.playersNatives.SetPlayerColor(this.Id, value.ToArgb());
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
        public int AnimationIndex
        {
            get
            {
                Guard.Disposal(this.Disposed);

                return this.playersNatives.GetPlayerAnimationIndex(this.Id);
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
        public Weaponstate Weaponstate
        {
            get
            {
                Guard.Disposal(this.Disposed);

                return (Weaponstate)this.playersNatives.GetPlayerWeaponState(this.Id);
            }
        }

        /// <inheritdoc />
        public Weapon CurrentWeapon
        {
            get
            {
                Guard.Disposal(this.Disposed);

                return (Weapon)this.playersNatives.GetPlayerWeapon(this.Id);
            }

            set
            {
                Guard.Disposal(this.Disposed);

                this.playersNatives.SetPlayerArmedWeapon(this.Id, (int)value);
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
        public FightStyle FightStyle
        {
            get
            {
                Guard.Disposal(this.Disposed);

                return (FightStyle)this.playersNatives.GetPlayerFightingStyle(this.Id);
            }

            set
            {
                Guard.Disposal(this.Disposed);

                this.playersNatives.SetPlayerFightingStyle(this.Id, (int)value);
            }
        }

        /// <inheritdoc />
        public bool PutPlayerIntoVehicle(IVehicle vehicle, int seat = 0)
        {
            Guard.Argument(vehicle, nameof(vehicle)).NotNull();
            Guard.Argument(seat, nameof(seat)).NotNegative();
            Guard.Disposal(this.Disposed);
            Guard.Disposal(vehicle.Disposed, nameof(vehicle));

            return this.playersNatives.PutPlayerInVehicle(this.Id, vehicle.Id, seat);
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
        public void Spawn()
        {
            Guard.Disposal(this.Disposed);

            this.playersNatives.SpawnPlayer(this.Id);
        }

        /// <inheritdoc />
        public int GetCurrentWeaponAmmo()
        {
            Guard.Disposal(this.Disposed);

            return this.playersNatives.GetPlayerAmmo(this.Id);
        }

        /// <inheritdoc />
        public void SetWeaponAmmo(Weapon weapon, int ammo)
        {
            Guard.Disposal(this.Disposed);

            this.playersNatives.SetPlayerAmmo(this.Id, (int)weapon, ammo);
        }

        /// <inheritdoc />
        public void GivePlayerWeapon(Weapon weapon, int ammo)
        {
            Guard.Disposal(this.Disposed);

            this.playersNatives.GivePlayerWeapon(this.Id, (int)weapon, ammo);
        }

        /// <inheritdoc />
        public void ResetWeapons()
        {
            Guard.Disposal(this.Disposed);

            this.playersNatives.ResetPlayerWeapons(this.Id);
        }

        /// <inheritdoc />
        public WeaponData GetWeaponData(int slot)
        {
            Guard.Disposal(this.Disposed);

            this.playersNatives.GetPlayerWeaponData(this.Id, slot, out var weapon, out var ammo);

            return new WeaponData((Weapon)weapon, ammo);
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
        public void SetSkillLevel(Weaponskill skill, int level)
        {
            Guard.Disposal(this.Disposed);

            this.playersNatives.SetPlayerSkillLevel(this.Id, (int)skill, level);
        }

        /// <inheritdoc />
        public int? GetSurfingVehicleId()
        {
            Guard.Disposal(this.Disposed);

            var vehicle = this.playersNatives.GetPlayerSurfingVehicleID(this.Id);

            if (vehicle == SampConstants.InvalidVehicleId)
            {
                return null;
            }

            return vehicle;
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
        public void GetLastShotVectors(out Vector3 originPosition, out Vector3 hitPosition)
        {
            Guard.Disposal(this.Disposed);

            this.playersNatives.GetPlayerLastShotVectors(
                                                         this.Id,
                                                         out var originX,
                                                         out var originY,
                                                         out var originZ,
                                                         out var hitPosX,
                                                         out var hitPosY,
                                                         out var hitPosZ);

            originPosition = new Vector3(originX, originY, originZ);
            hitPosition = new Vector3(hitPosX, hitPosY, hitPosZ);
        }

        /// <inheritdoc />
        public bool SetAttachedObject(
            int index,
            int modelid,
            int bone,
            Vector3 offset,
            Vector3 rotation,
            Vector3 scale,
            int materialColor1 = 0,
            int materialColor2 = 0)
        {
            Guard.Argument(index, nameof(index)).NotNegative().Max(PlayersConstants.MaxPlayerAttachedObjects);
            Guard.Disposal(this.Disposed);

            return this.playersNatives.SetPlayerAttachedObject(
                                                               this.Id,
                                                               index,
                                                               modelid,
                                                               bone,
                                                               offset.X,
                                                               offset.Y,
                                                               offset.Z,
                                                               rotation.X,
                                                               rotation.Y,
                                                               rotation.Z,
                                                               scale.X,
                                                               scale.Y,
                                                               scale.Z,
                                                               materialColor1,
                                                               materialColor2);
        }

        /// <inheritdoc />
        public void RemoveAttachedObject(int index)
        {
            Guard.Argument(index, nameof(index)).NotNegative().Max(PlayersConstants.MaxPlayerAttachedObjects);
            Guard.Disposal(this.Disposed);

            this.playersNatives.RemovePlayerAttachedObject(this.Id, index);
        }

        /// <inheritdoc />
        public bool IsAttachedObjectSlotUsed(int index)
        {
            Guard.Argument(index, nameof(index)).NotNegative().Max(PlayersConstants.MaxPlayerAttachedObjects);
            Guard.Disposal(this.Disposed);

            return this.playersNatives.IsPlayerAttachedObjectSlotUsed(this.Id, index);
        }

        /// <inheritdoc />
        public void EditAttachedObject(int index)
        {
            Guard.Argument(index, nameof(index)).NotNegative().Max(PlayersConstants.MaxPlayerAttachedObjects);
            Guard.Disposal(this.Disposed);

            this.playersNatives.EditAttachedObject(this.Id, index);
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
        public override string ToString()
        {
            return $"<{this.GetType()} ({this.Id}) {this.Name}>";
        }

        /// <inheritdoc />
        protected override void DisposeEntity()
        {
            this.entityRemoval(this);
        }
    }
}
