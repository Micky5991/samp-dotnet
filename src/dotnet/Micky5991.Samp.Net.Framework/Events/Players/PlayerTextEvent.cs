using Micky5991.EventAggregator.Elements;
using Micky5991.Samp.Net.Framework.Interfaces.Entities;

namespace Micky5991.Samp.Net.Framework.Events.Players
{
    /// <summary>
    /// Specialized event that will be triggered when a known player sends a chat message.
    /// </summary>
    public class PlayerTextEvent : CancellableEventBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PlayerTextEvent"/> class.
        /// </summary>
        /// <param name="player">Player that sent the message.</param>
        /// <param name="text">Message that has been sent.</param>
        public PlayerTextEvent(IPlayer player, string text)
        {
            this.Player = player;
            this.Text = text;
        }

        /// <summary>
        /// Gets the player that sent the message.
        /// </summary>
        public IPlayer Player { get; }

        /// <summary>
        /// Gets the message that has been sent.
        /// </summary>
        public string Text { get; }
    }
}
