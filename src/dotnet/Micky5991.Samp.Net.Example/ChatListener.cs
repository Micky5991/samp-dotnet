using Micky5991.EventAggregator;
using Micky5991.EventAggregator.Interfaces;
using Micky5991.Samp.Net.Core.Natives.Players;
using Micky5991.Samp.Net.Core.Natives.Samp;
using Micky5991.Samp.Net.Framework.Events.Samp;
using Micky5991.Samp.Net.Framework.Interfaces.Entities.Pools;
using Microsoft.Extensions.Logging;

namespace Micky5991.Samp.Net.Example
{
    public class ChatListener
    {
        private readonly IEventAggregator eventAggregator;

        private readonly ILogger<ChatListener> logger;

        private readonly IPlayersNatives playersNatives;

        private readonly ISampNatives sampNatives;

        private readonly IVehiclePool vehiclePool;

        private readonly IPlayerPool playerPool;

        public ChatListener(IEventAggregator eventAggregator, ILogger<ChatListener> logger, IPlayersNatives playersNatives, ISampNatives sampNatives, IVehiclePool vehiclePool, IPlayerPool playerPool)
        {
            this.eventAggregator = eventAggregator;
            this.logger = logger;
            this.playersNatives = playersNatives;
            this.sampNatives = sampNatives;
            this.vehiclePool = vehiclePool;
            this.playerPool = playerPool;
        }

        public void Attach()
        {
            this.eventAggregator.Subscribe<NativeGameModeInitEvent>(OnGamemodeInit);

            this.eventAggregator.Subscribe<PlayerTextEvent>(this.OnPlayerChat);
            this.eventAggregator.Subscribe<PlayerConnectEvent>(this.OnPlayerConnect);
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

        private void OnPlayerConnect(PlayerConnectEvent eventdata)
        {
            this.logger.LogInformation($"Player {eventdata.Player.Name} connected");
        }

        private void OnPlayerChat(PlayerTextEvent textEvent)
        {
            this.logger.LogInformation($"Incoming message: {textEvent.Player}: {textEvent.Text}");

            if (textEvent.Text == "veh")
            {
                var vehicle = this.vehiclePool.CreateVehicle(
                                                             Vehicle.Bullet,
                                                             textEvent.Player.Position,
                                                             0,
                                                             0,
                                                             150);

                textEvent.Player.PutPlayerIntoVehicle(vehicle, 0);
            }
        }

    }
}
