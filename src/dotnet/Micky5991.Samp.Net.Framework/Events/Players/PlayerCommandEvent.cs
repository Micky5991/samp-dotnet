using System;
using Dawn;
using Micky5991.EventAggregator.Elements;
using Micky5991.Samp.Net.Framework.Interfaces.Entities;

namespace Micky5991.Samp.Net.Framework.Events.Players
{
    /// <summary>
    /// Specialized event that will be triggered when a player executed a command.
    /// </summary>
    public class PlayerCommandEvent : CancellableEventBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PlayerCommandEvent"/> class.
        /// </summary>
        /// <param name="player">Player that sent the command.</param>
        /// <param name="commandText">Command that has been sent.</param>
        /// <exception cref="ArgumentNullException"><paramref name="player"/> or <paramref name="commandText"/> is null.</exception>
        /// <exception cref="ObjectDisposedException"><paramref name="player"/> was disposed.</exception>
        public PlayerCommandEvent(IPlayer player, string commandText)
        {
            Guard.Argument(player, nameof(player)).NotNull();
            Guard.Argument(commandText, nameof(commandText)).NotNull();
            Guard.Disposal(player.Disposed, nameof(player));

            this.Player = player;
            this.CommandText = commandText;
        }

        /// <summary>
        /// Gets the player that executed the command.
        /// </summary>
        public IPlayer Player { get; }

        /// <summary>
        /// Gets the text the player entered.
        /// </summary>
        public string CommandText { get; }
    }
}
