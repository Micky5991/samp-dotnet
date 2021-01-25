using System;
using Dawn;
using Micky5991.EventAggregator.Elements;
using Micky5991.Samp.Net.Framework.Interfaces.Entities;

namespace Micky5991.Samp.Net.Framework.Events.Samp
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
        /// <exception cref="ArgumentNullException"><paramref name="player"/> is null.</exception>
        /// <exception cref="ObjectDisposedException"><paramref name="player"/> was disposed.</exception>
        public PlayerConnectEvent(IPlayer player)
        {
            Guard.Argument(player, nameof(player)).NotNull();
            Guard.Disposal(player.Disposed, nameof(player));

            this.Player = player;
        }

        /// <summary>
        /// Gets player that connected to the server.
        /// </summary>
        public IPlayer Player { get; }
    }
}
