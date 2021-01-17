using Micky5991.EventAggregator;
using Micky5991.EventAggregator.Interfaces;
using Micky5991.Samp.Net.Core.Natives.Samp;
using Micky5991.Samp.Net.Framework.Events.Players;
using Micky5991.Samp.Net.Framework.Interfaces;
using Micky5991.Samp.Net.Framework.Interfaces.Pools;
using Microsoft.Extensions.Logging;

namespace Micky5991.Samp.Net.Framework.Entities.Listeners
{
    /// <summary>
    /// Attaches event handlers that handle common native events.
    /// </summary>
    public class PlayerEventListener : IEntityListener
    {
        private readonly IEventAggregator eventAggregator;

        private readonly ILogger<PlayerEventListener> logger;

        private readonly IPlayerPool playerPool;

        /// <summary>
        /// Initializes a new instance of the <see cref="PlayerEventListener"/> class.
        /// </summary>
        /// <param name="eventAggregator">Needed eventaggregator for this instance.</param>
        /// <param name="logger">Needed logger for this instance.</param>
        /// <param name="playerPool">Needed playerpool for this instance.</param>
        public PlayerEventListener(IEventAggregator eventAggregator, ILogger<PlayerEventListener> logger, IPlayerPool playerPool)
        {
            this.eventAggregator = eventAggregator;
            this.logger = logger;
            this.playerPool = playerPool;
        }

        /// <inheritdoc />
        public void Attach()
        {
            this.eventAggregator.Subscribe<NativePlayerTextEvent>(this.OnPlayerChat);
        }

        private void OnPlayerChat(NativePlayerTextEvent eventdata)
        {
            if (this.playerPool.Entities.TryGetValue(eventdata.Playerid, out var player) == false)
            {
                this.logger.LogWarning($"Received a {nameof(NativePlayerTextEvent)} from player {eventdata.Playerid}, but the player could not be found.");

                return;
            }

            var textEvent = this.eventAggregator.Publish(new PlayerTextEvent(player, eventdata.Text));

            eventdata.Cancelled = textEvent.Cancelled;
        }
    }
}
