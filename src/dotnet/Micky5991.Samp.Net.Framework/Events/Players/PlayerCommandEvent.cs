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
        public PlayerCommandEvent(IPlayer player, string commandText)
        {
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
