using Micky5991.EventAggregator;
using Micky5991.EventAggregator.Interfaces;
using Micky5991.Samp.Net.Core.Natives.Samp;
using Micky5991.Samp.Net.Framework.Enums;
using Micky5991.Samp.Net.Framework.Events.Samp;
using Micky5991.Samp.Net.Framework.Interfaces;
using Micky5991.Samp.Net.Framework.Interfaces.Pools;
using Microsoft.Extensions.Logging;

namespace Micky5991.Samp.Net.Framework.Elements.Entities.Listeners
{
    /// <inheritdoc />
    public class PlayerPoolListener : IEntityListener
    {
        private readonly IEventAggregator eventAggregator;

        private readonly IPlayerPool playerPool;

        private readonly ILogger<PlayerPoolListener> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="PlayerPoolListener"/> class.
        /// </summary>
        /// <param name="eventAggregator"><see cref="IEventAggregator"/> which receives needed player pool events.</param>
        /// <param name="playerPool">Current player pool instance.</param>
        /// <param name="logger">Logger for this listener.</param>
        public PlayerPoolListener(IEventAggregator eventAggregator, IPlayerPool playerPool, ILogger<PlayerPoolListener> logger)
        {
            this.eventAggregator = eventAggregator;
            this.playerPool = playerPool;
            this.logger = logger;
        }

        /// <inheritdoc />
        public virtual void Attach()
        {
            this.eventAggregator.Subscribe<NativePlayerConnectEvent>(this.OnPlayerConnect, eventPriority: EventPriority.Lowest);
            this.eventAggregator.Subscribe<NativePlayerDisconnectEvent>(this.OnPlayerDisconnect, eventPriority: EventPriority.Lowest);
        }

        private void OnPlayerConnect(NativePlayerConnectEvent eventdata)
        {
            var player = this.playerPool.CreateAndAddPlayer(eventdata.Playerid);

            var connectEvent = this.eventAggregator.Publish(new PlayerConnectEvent(player));

            eventdata.Cancelled = connectEvent.Cancelled;
        }

        private void OnPlayerDisconnect(NativePlayerDisconnectEvent eventdata)
        {
            var player = this.playerPool.RemovePlayer(eventdata.Playerid);
            if (player == null)
            {
                this.logger.LogWarning($"Player {eventdata.Playerid} disconnected, but no instance could be found in {nameof(IPlayerPool)}.");

                return;
            }

            var disconnectEvent = this.eventAggregator.Publish(new PlayerDisconnectEvent(player, (DisconnectReason)eventdata.Reason));

            eventdata.Cancelled = disconnectEvent.Cancelled;

            player.Dispose();
        }
    }
}
