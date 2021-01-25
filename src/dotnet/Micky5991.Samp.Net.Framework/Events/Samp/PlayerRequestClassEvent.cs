using Dawn;
using Micky5991.EventAggregator.Elements;
using Micky5991.Samp.Net.Framework.Interfaces.Entities;

namespace Micky5991.Samp.Net.Framework.Events.Samp
{
    /// <summary>
    /// Event that will be triggered when the player requests a class on spawn.
    /// </summary>
    public class PlayerRequestClassEvent : CancellableEventBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PlayerRequestClassEvent"/> class.
        /// </summary>
        /// <param name="player">Player that requested the class.</param>
        /// <param name="classId">Id of the class that has been requested.</param>
        public PlayerRequestClassEvent(IPlayer player, int classId)
        {
            Guard.Argument(player, nameof(player)).NotNull();
            Guard.Disposal(player.Disposed, nameof(player));

            this.Player = player;
            this.ClassId = classId;
        }

        /// <summary>
        /// Gets the player that requested the class.
        /// </summary>
        public IPlayer Player { get; }

        /// <summary>
        /// Gets the id of the class that has been requested.
        /// </summary>
        public int ClassId { get; }
    }
}
