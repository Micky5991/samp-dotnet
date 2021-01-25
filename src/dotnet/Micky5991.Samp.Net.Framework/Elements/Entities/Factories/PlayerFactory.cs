using Micky5991.Samp.Net.Core.Natives.Players;
using Micky5991.Samp.Net.Core.Natives.Samp;
using Micky5991.Samp.Net.Framework.Interfaces.Entities;
using Micky5991.Samp.Net.Framework.Interfaces.Factories;
using Micky5991.Samp.Net.Framework.Interfaces.Pools;

namespace Micky5991.Samp.Net.Framework.Elements.Entities.Factories
{
    /// <inheritdoc />
    public class PlayerFactory : IPlayerFactory
    {
        private readonly IPlayersNatives playersNatives;

        private readonly ISampNatives sampNatives;

        /// <summary>
        /// Initializes a new instance of the <see cref="PlayerFactory"/> class.
        /// </summary>
        /// <param name="sampNatives">General Samp natives needed for <see cref="Player"/>.</param>
        /// <param name="playersNatives">Natives needed for every <see cref="Player"/> instance.</param>
        public PlayerFactory(ISampNatives sampNatives, IPlayersNatives playersNatives)
        {
            this.sampNatives = sampNatives;
            this.playersNatives = playersNatives;
        }

        /// <inheritdoc />
        public IPlayer CreatePlayer(int playerid, IPlayerPool.RemoveEntityDelegate removeEntity)
        {
            return new Player(playerid, removeEntity, this.sampNatives, this.playersNatives);
        }
    }
}
