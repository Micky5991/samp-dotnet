using System;
using Dawn;
using Micky5991.EventAggregator.Elements;
using Micky5991.Samp.Net.Framework.Interfaces.Entities;

namespace Micky5991.Samp.Net.Framework.Events.Samp
{
    /// <summary>
    /// Event that will be triggered when a player died.
    /// </summary>
    public class PlayerDeathEvent : CancellableEventBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PlayerDeathEvent"/> class.
        /// </summary>
        /// <param name="player">Player that has been killed.</param>
        /// <param name="killer">Optional killer that killed the <paramref name="player"/>.</param>
        /// <param name="reason">Numerical reason of death.</param>
        /// <exception cref="ArgumentNullException"><paramref name="player"/> is null.</exception>
        /// <exception cref="ObjectDisposedException"><paramref name="player"/> or <paramref name="killer"/> was disposed.</exception>
        public PlayerDeathEvent(IPlayer player, IPlayer? killer, int reason)
        {
            Guard.Argument(player, nameof(player)).NotNull();
            Guard.Disposal(player.Disposed, nameof(player));

            if (killer != null)
            {
                Guard.Disposal(killer.Disposed, nameof(player));
            }

            this.Player = player;
            this.Killer = killer;
            this.Reason = reason;
        }

        /// <summary>
        /// Gets the player that died.
        /// </summary>
        public IPlayer Player { get; }

        /// <summary>
        /// Gets the optional killer of this <see cref="Player"/>.
        /// </summary>
        public IPlayer? Killer { get; }

        /// <summary>
        /// Gets the reason why the player died.
        /// </summary>
        public int Reason { get; }
    }
}
