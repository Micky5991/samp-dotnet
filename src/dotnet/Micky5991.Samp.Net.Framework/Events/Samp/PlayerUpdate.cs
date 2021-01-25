using Micky5991.EventAggregator.Elements;
using Micky5991.Samp.Net.Framework.Interfaces.Entities;

namespace Micky5991.Samp.Net.Framework.Events.Samp
{
    /// <summary>
    /// Event that will be triggered when a player update was sent to the server.
    /// </summary>
    public class PlayerUpdate : CancellableEventBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PlayerUpdate"/> class.
        /// </summary>
        /// <param name="player">Player that has been updated.</param>
        public PlayerUpdate(IPlayer player)
        {
            this.Player = player;
        }

        /// <summary>
        /// Gets the player that has been updated.
        /// </summary>
        public IPlayer Player { get; }
    }
}
