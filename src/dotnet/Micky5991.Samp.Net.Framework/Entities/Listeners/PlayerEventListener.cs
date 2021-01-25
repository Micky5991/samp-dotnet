using Micky5991.EventAggregator.Interfaces;
using Micky5991.Samp.Net.Core.Natives.Samp;
using Micky5991.Samp.Net.Framework.Events.Samp;
using Micky5991.Samp.Net.Framework.Interfaces.Entities;
using Micky5991.Samp.Net.Framework.Interfaces.Pools;
using Microsoft.Extensions.Logging;

namespace Micky5991.Samp.Net.Framework.Entities.Listeners
{
    /// <summary>
    /// Attaches event handlers that handle common native events.
    /// </summary>
    public class PlayerEventListener : EventListenerBase
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
            : base(eventAggregator)
        {
            this.eventAggregator = eventAggregator;
            this.logger = logger;
            this.playerPool = playerPool;
        }

        /// <inheritdoc />
        public override void Attach()
        {
            this.eventAggregator.Subscribe<NativePlayerTextEvent>(this.OnPlayerChat);
            this.eventAggregator.Subscribe<NativePlayerCommandTextEvent>(this.OnPlayerCommand);
            this.eventAggregator.Subscribe<NativePlayerDeathEvent>(this.OnPlayerDeath);
            this.eventAggregator.Subscribe<NativePlayerRequestClassEvent>(this.OnPlayerRequestClass);
            this.eventAggregator.Subscribe<NativePlayerRequestSpawnEvent>(this.OnPlayerRequestSpawn);
            this.eventAggregator.Subscribe<NativePlayerSpawnEvent>(this.OnPlayerSpawn);
            this.eventAggregator.Subscribe<NativePlayerUpdateEvent>(this.OnPlayerUpdate);
            this.eventAggregator.Subscribe<NativePlayerStateChangeEvent>(this.OnPlayerStateChange);
        }

        private void OnPlayerStateChange(NativePlayerStateChangeEvent eventdata)
        {
            if (this.playerPool.Entities.TryGetValue(eventdata.Playerid, out var player) == false)
            {
                this.logger.LogWarning($"Received a {nameof(NativePlayerUpdateEvent)} from player {eventdata.Playerid}, but the player could not be found.");

                return;
            }

            this.eventAggregator.Publish(new PlayerStateChangeEvent(player, (PlayerState)eventdata.Oldstate, (PlayerState)eventdata.Newstate));
        }

        private void OnPlayerUpdate(NativePlayerUpdateEvent eventdata)
        {
            if (this.playerPool.Entities.TryGetValue(eventdata.Playerid, out var player) == false)
            {
                this.logger.LogWarning($"Received a {nameof(NativePlayerUpdateEvent)} from player {eventdata.Playerid}, but the player could not be found.");

                return;
            }

            this.WrapCancellableEvent(eventdata, new PlayerUpdateEvent(player));
        }

        private void OnPlayerSpawn(NativePlayerSpawnEvent eventdata)
        {
            if (this.playerPool.Entities.TryGetValue(eventdata.Playerid, out var player) == false)
            {
                this.logger.LogWarning($"Received a {nameof(NativePlayerSpawnEvent)} from player {eventdata.Playerid}, but the player could not be found.");

                return;
            }

            this.WrapCancellableEvent(eventdata, new PlayerSpawnEvent(player));
        }

        private void OnPlayerRequestSpawn(NativePlayerRequestSpawnEvent eventdata)
        {
            if (this.playerPool.Entities.TryGetValue(eventdata.Playerid, out var player) == false)
            {
                this.logger.LogWarning($"Received a {nameof(NativePlayerRequestSpawnEvent)} from player {eventdata.Playerid}, but the player could not be found.");

                return;
            }

            this.WrapCancellableEvent(eventdata, new PlayerRequestSpawnEvent(player));
        }

        private void OnPlayerRequestClass(NativePlayerRequestClassEvent eventdata)
        {
            if (this.playerPool.Entities.TryGetValue(eventdata.Playerid, out var player) == false)
            {
                this.logger.LogWarning($"Received a {nameof(NativePlayerRequestClassEvent)} from player {eventdata.Playerid}, but the player could not be found.");

                return;
            }

            this.eventAggregator.Publish(new PlayerRequestClassEvent(player, eventdata.Classid));
        }

        private void OnPlayerDeath(NativePlayerDeathEvent eventdata)
        {
            if (this.playerPool.Entities.TryGetValue(eventdata.Playerid, out var player) == false)
            {
                this.logger.LogWarning($"Received a {nameof(NativePlayerDeathEvent)} from player {eventdata.Playerid}, but the player could not be found.");

                return;
            }

            IPlayer? killer = null;

            if (eventdata.Killerid != SampConstants.InvalidPlayerId)
            {
                if (this.playerPool.Entities.TryGetValue(eventdata.Killerid, out killer) == false)
                {
                    this.logger.LogWarning($"Received a {nameof(NativePlayerDeathEvent)} from killer {eventdata.Killerid}, but the player could not be found.");

                    return;
                }
            }

            this.WrapCancellableEvent(eventdata, new PlayerDeathEvent(player, killer, eventdata.Reason));
        }

        private void OnPlayerCommand(NativePlayerCommandTextEvent eventdata)
        {
            if (this.playerPool.Entities.TryGetValue(eventdata.Playerid, out var player) == false)
            {
                this.logger.LogWarning($"Received a {nameof(NativePlayerCommandTextEvent)} from player {eventdata.Playerid}, but the player could not be found.");

                return;
            }

            this.WrapCancellableEvent(eventdata, new PlayerCommandEvent(player, eventdata.Cmdtext));
        }

        private void OnPlayerChat(NativePlayerTextEvent eventdata)
        {
            if (this.playerPool.Entities.TryGetValue(eventdata.Playerid, out var player) == false)
            {
                this.logger.LogWarning($"Received a {nameof(NativePlayerTextEvent)} from player {eventdata.Playerid}, but the player could not be found.");

                return;
            }

            this.WrapCancellableEvent(eventdata, new PlayerTextEvent(player, eventdata.Text));
        }
    }
}
