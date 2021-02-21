using Micky5991.EventAggregator;
using Micky5991.EventAggregator.Interfaces;
using Micky5991.Samp.Net.Core.Natives.Players;
using Micky5991.Samp.Net.Core.Natives.Samp;
using Micky5991.Samp.Net.Framework.Elements.Dialogs;
using Micky5991.Samp.Net.Framework.Events.Samp;
using Micky5991.Samp.Net.Framework.Interfaces.Services;
using Microsoft.Extensions.Logging;

namespace Micky5991.Samp.Net.Example
{
    public class ChatListener
    {
        private readonly IEventAggregator eventAggregator;

        private readonly ILogger<ChatListener> logger;

        private readonly IPlayersNatives playersNatives;

        private readonly ISampNatives sampNatives;

        private readonly IDialogHandler dialogHandler;

        public ChatListener(
            IEventAggregator eventAggregator,
            ILogger<ChatListener> logger,
            IPlayersNatives playersNatives,
            ISampNatives sampNatives,
            IDialogHandler dialogHandler)
        {
            this.eventAggregator = eventAggregator;
            this.logger = logger;
            this.playersNatives = playersNatives;
            this.sampNatives = sampNatives;
            this.dialogHandler = dialogHandler;
        }

        public void Attach()
        {
            this.eventAggregator.Subscribe<NativeGameModeInitEvent>(OnGamemodeInit);

            this.eventAggregator.Subscribe<PlayerConnectEvent>(this.OnPlayerConnect);
            this.eventAggregator.Subscribe<PlayerRequestClassEvent>(this.OnPlayerRequestClass);
            this.eventAggregator.Subscribe<NativePlayerRequestSpawnEvent>(this.OnPlayerRequestSpawn);
            this.eventAggregator.Subscribe<NativePlayerSpawnEvent>(this.OnPlayerSpawn, eventPriority: EventPriority.Highest);
        }

        private void OnGamemodeInit(NativeGameModeInitEvent eventdata)
        {
            this.sampNatives.DisableInteriorEnterExits();
        }

        private void OnPlayerSpawn(NativePlayerSpawnEvent eventdata)
        {
            this.logger.LogInformation($"Player {eventdata.Playerid} spawned");

            eventdata.Cancelled = false;
        }

        private void OnPlayerRequestSpawn(NativePlayerRequestSpawnEvent eventdata)
        {
            this.logger.LogInformation($"Player {eventdata.Playerid} requested spawn");
            var success = this.playersNatives.SpawnPlayer(eventdata.Playerid);

            // eventdata.Cancelled = true;
        }

        private void OnPlayerRequestClass(PlayerRequestClassEvent eventdata)
        {
            eventdata.Player.Spawn();
        }

        private void OnPlayerConnect(PlayerConnectEvent eventdata)
        {
            eventdata.Player.HideDialogs();

            this.logger.LogInformation($"Player {eventdata.Player.Name} connected");
        }
    }
}
