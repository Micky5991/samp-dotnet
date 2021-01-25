using Micky5991.EventAggregator.Elements;
using Micky5991.Samp.Net.Framework.Interfaces.Entities;

namespace Micky5991.Samp.Net.Framework.Events.Samp
{
    /// <summary>
    /// Event that will be triggered when a player update was sent to the server.
    /// </summary>
    public class PlayerUpdateEvent : CancellableEventBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PlayerUpdateEvent"/> class.
        /// </summary>
        /// <param name="player">Player that has been updated.</param>
        public PlayerUpdateEvent(IPlayer player)
        {
            this.Player = player;
        }

        /// <summary>
        /// Gets the player that has been updated.
        /// </summary>
        public IPlayer Player { get; }
    }
}
