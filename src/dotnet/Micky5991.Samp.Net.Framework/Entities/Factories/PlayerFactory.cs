using Micky5991.Samp.Net.Core.Natives.Players;
using Micky5991.Samp.Net.Framework.Interfaces.Entities;
using Micky5991.Samp.Net.Framework.Interfaces.Factories;
using Micky5991.Samp.Net.Framework.Interfaces.Pools;

namespace Micky5991.Samp.Net.Framework.Entities.Factories
{
    /// <inheritdoc />
    public class PlayerFactory : IPlayerFactory
    {
        private readonly IPlayersNatives playersNatives;

        /// <summary>
        /// Initializes a new instance of the <see cref="PlayerFactory"/> class.
        /// </summary>
        /// <param name="playersNatives">Natives needed for every <see cref="Player"/> instance.</param>
        public PlayerFactory(IPlayersNatives playersNatives)
        {
            this.playersNatives = playersNatives;
        }

        /// <inheritdoc />
        public IPlayer CreatePlayer(int playerid, IPlayerPool.RemoveEntityDelegate removeEntity)
        {
            return new Player(playerid, removeEntity, this.playersNatives);
        }
    }
}
