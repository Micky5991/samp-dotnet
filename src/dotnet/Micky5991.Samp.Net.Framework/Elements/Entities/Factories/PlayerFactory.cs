using Micky5991.EventAggregator.Interfaces;
using Micky5991.Samp.Net.Core.Natives.Players;
using Micky5991.Samp.Net.Core.Natives.Samp;
using Micky5991.Samp.Net.Framework.Interfaces.Entities;
using Micky5991.Samp.Net.Framework.Interfaces.Entities.Factories;
using Micky5991.Samp.Net.Framework.Interfaces.Entities.Pools;

namespace Micky5991.Samp.Net.Framework.Elements.Entities.Factories
{
    /// <inheritdoc />
    public class PlayerFactory : IPlayerFactory
    {
        private readonly IPlayersNatives playersNatives;

        private readonly IEventAggregator eventAggregator;

        private readonly ISampNatives sampNatives;

        /// <summary>
        /// Initializes a new instance of the <see cref="PlayerFactory"/> class.
        /// </summary>
        /// <param name="sampNatives">General Samp natives needed for <see cref="Player"/>.</param>
        /// <param name="playersNatives">Natives needed for every <see cref="Player"/> instance.</param>
        /// <param name="eventAggregator">Event Aggregator needed to build player.</param>
        public PlayerFactory(
            ISampNatives sampNatives,
            IPlayersNatives playersNatives,
            IEventAggregator eventAggregator)
        {
            this.sampNatives = sampNatives;
            this.playersNatives = playersNatives;
            this.eventAggregator = eventAggregator;
        }

        /// <inheritdoc />
        public IPlayer CreatePlayer(int playerid, IPlayerPool.RemoveEntityDelegate removeEntity)
        {
            return new Player(
                              playerid,
                              removeEntity,
                              this.sampNatives,
                              this.playersNatives,
                              this.eventAggregator);
        }
    }
}
