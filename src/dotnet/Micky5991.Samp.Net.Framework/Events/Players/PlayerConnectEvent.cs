using Micky5991.EventAggregator.Elements;
using Micky5991.Samp.Net.Framework.Interfaces.Entities;

namespace Micky5991.Samp.Net.Framework.Events.Players
{
    /// <summary>
    /// Specialized event that will be triggered when a player connected.
    /// </summary>
    public class PlayerConnectEvent : CancellableEventBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PlayerConnectEvent"/> class.
        /// </summary>
        /// <param name="player">Player that connected.</param>
        public PlayerConnectEvent(IPlayer player)
        {
            this.Player = player;
        }

        /// <summary>
        /// Gets player that connected to the server.
        /// </summary>
        public IPlayer Player { get; }
    }
}
