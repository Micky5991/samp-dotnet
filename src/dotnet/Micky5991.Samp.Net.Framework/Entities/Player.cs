using System.Drawing;
using System.Numerics;
using Dawn;
using Micky5991.Samp.Net.Core.Natives.Players;
using Micky5991.Samp.Net.Core.Natives.Samp;
using Micky5991.Samp.Net.Framework.Interfaces.Entities;
using Micky5991.Samp.Net.Framework.Interfaces.Pools;

namespace Micky5991.Samp.Net.Framework.Entities
{
    /// <inheritdoc cref="IPlayer"/>
    public class Player : Entity, IPlayer
    {
        private readonly IPlayerPool.RemoveEntityDelegate entityRemoval;

        private readonly IPlayersNatives playersNatives;

        /// <summary>
        /// Initializes a new instance of the <see cref="Player"/> class.
        /// </summary>
        /// <param name="id">The id of this player.</param>
        /// <param name="entityRemoval">Pool removal delgate.</param>
        /// <param name="playersNatives">Natives needed for this entity.</param>
        public Player(int id, IPlayerPool.RemoveEntityDelegate entityRemoval, IPlayersNatives playersNatives)
            : base(id)
        {
            this.entityRemoval = entityRemoval;
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
        public bool PutPlayerIntoVehicle(IVehicle vehicle, int seat = 0)
        {
            Guard.Argument(vehicle, nameof(vehicle)).NotNull();
            Guard.Argument(seat, nameof(seat)).NotNegative();
            Guard.Disposal(vehicle.Disposed);

            return this.playersNatives.PutPlayerInVehicle(this.Id, vehicle.Id, seat);
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
