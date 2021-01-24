using Micky5991.EventAggregator.Elements;
using Micky5991.Samp.Net.Framework.Interfaces.Entities;

namespace Micky5991.Samp.Net.Framework.Events.Players
{
    /// <summary>
    /// Event that will be triggered when a player requests a spawn on class selection.
    /// </summary>
    public class PlayerRequestSpawn : CancellableEventBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PlayerRequestSpawn"/> class.
        /// </summary>
        /// <param name="player">Player that requested the spawn.</param>
        public PlayerRequestSpawn(IPlayer player)
        {
            this.Player = player;
        }

        /// <summary>
        /// Gets the player that requested the spawn.
        /// </summary>
        public IPlayer Player { get; }
    }
}
