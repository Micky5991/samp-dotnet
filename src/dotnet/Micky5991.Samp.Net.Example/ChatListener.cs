using Micky5991.EventAggregator;
using Micky5991.EventAggregator.Interfaces;
using Micky5991.Samp.Net.Core.Natives.Players;
using Micky5991.Samp.Net.Core.Natives.Samp;
using Micky5991.Samp.Net.Framework.Elements.Dialogs;
using Micky5991.Samp.Net.Framework.Events.Samp;
using Micky5991.Samp.Net.Framework.Interfaces.Entities.Pools;
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

        private readonly IVehiclePool vehiclePool;

        private readonly IPlayerPool playerPool;

        private readonly IDialogHandler dialogHandler;

        public ChatListener(
            IEventAggregator eventAggregator,
            ILogger<ChatListener> logger,
            IPlayersNatives playersNatives,
            ISampNatives sampNatives,
            IVehiclePool vehiclePool,
            IPlayerPool playerPool,
            IDialogHandler dialogHandler)
        {
            this.eventAggregator = eventAggregator;
            this.logger = logger;
            this.playersNatives = playersNatives;
            this.sampNatives = sampNatives;
            this.vehiclePool = vehiclePool;
            this.playerPool = playerPool;
            this.dialogHandler = dialogHandler;
        }

        public void Attach()
        {
            this.eventAggregator.Subscribe<NativeGameModeInitEvent>(OnGamemodeInit);

            this.eventAggregator.Subscribe<PlayerTextEvent>(this.OnPlayerChat);
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

        private async void OnPlayerRequestClass(PlayerRequestClassEvent eventdata)
        {
            this.logger.LogInformation($"Player {eventdata.Player} requested class");

            var dialog = new TextInputDialog();
            dialog.SetCaption("Hey");
            dialog.SetMessage("Login pls");
            dialog.SetButtons("Login");

            var response = await this.dialogHandler.ShowDialogAsync(eventdata.Player, dialog);

            this.logger.LogInformation($"Text: {response.InputText}");

            eventdata.Player.Spawn();
        }

        private void OnPlayerConnect(PlayerConnectEvent eventdata)
        {
            eventdata.Player.HideDialogs();

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
