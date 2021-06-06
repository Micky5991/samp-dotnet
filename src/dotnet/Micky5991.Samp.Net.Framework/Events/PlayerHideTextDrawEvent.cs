using Micky5991.EventAggregator.Elements;
using Micky5991.Samp.Net.Framework.Interfaces.Entities;
using Micky5991.Samp.Net.Framework.Interfaces.TextDraws;

namespace Micky5991.Samp.Net.Framework.Events
{
    /// <summary>
    /// Event that triggers to signal the corresponding handler to display the given TextDraw.
    /// </summary>
    public class PlayerHideTextDrawEvent : EventBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PlayerHideTextDrawEvent"/> class.
        /// </summary>
        /// <param name="player">Player to show the <see cref="TextDraw"/> to.</param>
        /// <param name="textDraw">Textdraw to display.</param>
        public PlayerHideTextDrawEvent(IPlayer player, ITextDraw textDraw)
        {
            this.Player = player;
            this.TextDraw = textDraw;
        }

        /// <summary>
        /// Gets the player to show the <see cref="TextDraw"/> to.
        /// </summary>
        public IPlayer Player { get; }

        /// <summary>
        /// Gets the textdraw that should be shown.
        /// </summary>
        public ITextDraw TextDraw { get; }
    }
}
