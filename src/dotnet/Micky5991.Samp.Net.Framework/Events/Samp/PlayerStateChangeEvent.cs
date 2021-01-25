using Micky5991.EventAggregator.Elements;
using Micky5991.Samp.Net.Core.Natives.Samp;
using Micky5991.Samp.Net.Framework.Interfaces.Entities;

namespace Micky5991.Samp.Net.Framework.Events.Samp
{
    /// <summary>
    /// Event that will be triggered when the player state changed.
    /// </summary>
    public class PlayerStateChangeEvent : EventBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PlayerStateChangeEvent"/> class.
        /// </summary>
        /// <param name="player">Player that changed the state.</param>
        /// <param name="oldState">Old state of the player.</param>
        /// <param name="newState">New state of the player.</param>
        public PlayerStateChangeEvent(IPlayer player, PlayerState oldState, PlayerState newState)
        {
            this.Player = player;
            this.OldState = oldState;
            this.NewState = newState;
        }

        /// <summary>
        /// Gets the player that changed the state.
        /// </summary>
        public IPlayer Player { get; }

        /// <summary>
        /// Gets the old state of the player.
        /// </summary>
        public PlayerState OldState { get; }

        /// <summary>
        /// Gets the new state of the player.
        /// </summary>
        public PlayerState NewState { get; }
    }
}
