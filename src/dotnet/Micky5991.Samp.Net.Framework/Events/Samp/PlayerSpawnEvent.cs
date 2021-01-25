using System;
using Dawn;
using Micky5991.EventAggregator.Elements;
using Micky5991.Samp.Net.Framework.Interfaces.Entities;

namespace Micky5991.Samp.Net.Framework.Events.Samp
{
    /// <summary>
    /// Event that will be triggered when a player spawned.
    /// </summary>
    public class PlayerSpawnEvent : CancellableEventBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PlayerSpawnEvent"/> class.
        /// </summary>
        /// <param name="player">Player that spawned.</param>
        /// <exception cref="ArgumentNullException"><paramref name="player"/> is null.</exception>
        /// <exception cref="ObjectDisposedException"><paramref name="player"/> was disposed.</exception>
        public PlayerSpawnEvent(IPlayer player)
        {
            Guard.Argument(player, nameof(player)).NotNull();
            Guard.Disposal(player.Disposed, nameof(player));

            this.Player = player;
        }

        /// <summary>
        /// Gets the player that spawned.
        /// </summary>
        public IPlayer Player { get; }
    }
}
