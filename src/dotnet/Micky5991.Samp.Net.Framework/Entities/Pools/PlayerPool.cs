using Micky5991.Samp.Net.Framework.Interfaces.Entities;
using Micky5991.Samp.Net.Framework.Interfaces.Factories;
using Micky5991.Samp.Net.Framework.Interfaces.Pools;

namespace Micky5991.Samp.Net.Framework.Entities.Pools
{
    /// <summary>
    /// Implements a player handler.
    /// </summary>
    public class PlayerPool : EntityPool<IPlayer>, IPlayerPool
    {
        private readonly IPlayerFactory playerFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="PlayerPool"/> class.
        /// </summary>
        /// <param name="playerFactory">Factory that creates <see cref="IPlayer"/> instances on demand.</param>
        public PlayerPool(IPlayerFactory playerFactory)
        {
            this.playerFactory = playerFactory;
        }

        /// <inheritdoc />
        public IPlayer AddPlayer(int playerid)
        {
            var player = this.playerFactory.CreatePlayer(playerid, this.RemoveEntity);

            this.AddEntity(player);

            return player;
        }

        /// <inheritdoc />
        public IPlayer? RemovePlayer(int playerid)
        {
            IPlayer? result = null;

            this.UpdateEntities(c => c.TryGetValue(playerid, out result) ? c.Remove(playerid) : c);

            return result;
        }
    }
}
