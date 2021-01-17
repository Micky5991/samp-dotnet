using System.Diagnostics;
using Micky5991.EventAggregator;
using Micky5991.EventAggregator.Interfaces;
using Micky5991.Samp.Net.Core.Interop;
using Micky5991.Samp.Net.Core.Natives.Players;
using Micky5991.Samp.Net.Core.Natives.Samp;
using Microsoft.Extensions.Logging;

namespace Micky5991.Samp.Net.Example
{
    public class ChatListener
    {
        private readonly IEventAggregator eventAggregator;

        private readonly ILogger<ChatListener> logger;

        private readonly IPlayersNatives playersNatives;

        private readonly ISampNatives sampNatives;

        public ChatListener(IEventAggregator eventAggregator, ILogger<ChatListener> logger, IPlayersNatives playersNatives, ISampNatives sampNatives)
        {
            this.eventAggregator = eventAggregator;
            this.logger = logger;
            this.playersNatives = playersNatives;
            this.sampNatives = sampNatives;
        }

        public void Attach()
        {
            this.eventAggregator.Subscribe<NativeGameModeInitEvent>(OnGamemodeInit);

            this.eventAggregator.Subscribe<NativePlayerTextEvent>(this.OnPlayerChat);
            this.eventAggregator.Subscribe<NativePlayerConnectEvent>(this.OnPlayerConnect);
            this.eventAggregator.Subscribe<NativePlayerRequestClassEvent>(this.OnPlayerRequestClass);
            this.eventAggregator.Subscribe<NativePlayerRequestSpawnEvent>(this.OnPlayerRequestSpawn);
            this.eventAggregator.Subscribe<NativeDialogResponseEvent>(OnPlayerRespondDialog);
            this.eventAggregator.Subscribe<NativePlayerSpawnEvent>(OnPlayerSpawn, eventPriority: EventPriority.Highest);
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

        private void OnPlayerRespondDialog(NativeDialogResponseEvent eventdata)
        {
            if (eventdata.Dialogid == 2)
            {
                this.logger.LogInformation($"Spawn player {eventdata.Playerid}");
                this.playersNatives.SpawnPlayer(eventdata.Playerid);
            }
        }

        private void OnPlayerRequestSpawn(NativePlayerRequestSpawnEvent eventdata)
        {
            this.logger.LogInformation($"Player {eventdata.Playerid} requested spawn");
            var success = this.playersNatives.SpawnPlayer(eventdata.Playerid);

            // eventdata.Cancelled = true;
        }

        private void OnPlayerRequestClass(NativePlayerRequestClassEvent eventdata)
        {
            this.logger.LogInformation($"Player {eventdata.Playerid} requested class");
            this.playersNatives.SpawnPlayer(eventdata.Playerid);

            this.sampNatives.ShowPlayerDialog(eventdata.Playerid, 2, SampConstants.DialogStylePassword, "Test", "COOL", "OK", "abort");

            // eventdata.Cancelled = true;
        }

        private void OnPlayerConnect(NativePlayerConnectEvent eventdata)
        {
            this.logger.LogInformation($"Player {eventdata.Playerid} connected");
        }

        private void OnPlayerChat(NativePlayerTextEvent textEvent)
        {
            this.logger.LogInformation($"Incoming message: {textEvent.Playerid}: {textEvent.Text}");
        }

    }
}
