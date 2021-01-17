using System.Numerics;
using Micky5991.Samp.Net.Core.Natives.Players;
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
                this.playersNatives.GetPlayerPos(this.Id, out var x, out var y, out var z);

                return new Vector3(x, y, z);
            }
            set => this.playersNatives.SetPlayerPos(this.Id, value.X, value.Y, value.Z);
        }

        /// <inheritdoc />
        protected override void DisposeEntity()
        {
            this.entityRemoval(this);
        }
    }
}
