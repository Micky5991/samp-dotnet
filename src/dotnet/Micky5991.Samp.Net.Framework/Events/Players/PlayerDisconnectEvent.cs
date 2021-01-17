using Micky5991.EventAggregator.Elements;
using Micky5991.Samp.Net.Core.Natives.Players;
using Micky5991.Samp.Net.Core.Natives.Samp;
using Micky5991.Samp.Net.Framework.Enums;
using Micky5991.Samp.Net.Framework.Interfaces.Entities;

namespace Micky5991.Samp.Net.Framework.Events.Players
{
    /// <summary>
    /// Specialized event that will be triggered when a player disconnected from the server.
    /// </summary>
    public class PlayerDisconnectEvent : CancellableEventBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PlayerDisconnectEvent"/> class.
        /// </summary>
        /// <param name="player">Player that disconnected.</param>
        /// <param name="reason">Reason why the player disconnected.</param>
        public PlayerDisconnectEvent(IPlayer player, DisconnectReason reason)
        {
            this.Player = player;
            this.Reason = reason;
        }

        /// <summary>
        /// Gets the player that disconnected.
        /// </summary>
        public IPlayer Player { get; }

        /// <summary>
        /// Gets the reason why the player disconnected.
        /// </summary>
        public DisconnectReason Reason { get; }
    }
}
