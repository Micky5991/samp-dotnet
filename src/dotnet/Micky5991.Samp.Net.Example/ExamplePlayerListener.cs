using System;
using System.Threading.Tasks;
using Micky5991.EventAggregator.Interfaces;
using Micky5991.Samp.Net.Core.Natives.Samp;
using Micky5991.Samp.Net.Framework.Events.Samp;
using Micky5991.Samp.Net.Framework.Interfaces;

namespace Micky5991.Samp.Net.Example
{
    public class ExamplePlayerListener : IEventListener
    {
        private readonly IEventAggregator eventAggregator;

        private readonly ISampNatives sampNatives;

        public ExamplePlayerListener(
            IEventAggregator eventAggregator,
            ISampNatives sampNatives)
        {
            this.eventAggregator = eventAggregator;
            this.sampNatives = sampNatives;
        }

        public void Attach()
        {
            this.eventAggregator.Subscribe<NativeGameModeInitEvent>(this.OnGamemodeInit);

            this.eventAggregator.Subscribe<PlayerConnectEvent>(this.OnPlayerConnect);
            this.eventAggregator.Subscribe<PlayerRequestClassEvent>(this.OnPlayerRequestClass);
            this.eventAggregator.Subscribe<PlayerDeathEvent>(this.OnPlayerDeath);
        }

        private async Task OnPlayerDeath(PlayerDeathEvent eventdata)
        {
            var player = eventdata.Player;

            player.SetSpawnInfo(0, 0, player.Position, player.Rotation);

            await Task.Delay(TimeSpan.FromSeconds(2));
        }

        private void OnGamemodeInit(NativeGameModeInitEvent eventdata)
        {
            this.sampNatives.DisableInteriorEnterExits();
            this.sampNatives.AddPlayerClass(
                                            0,
                                            1541.4635f,
                                            -1675.2650f,
                                            13.5526f,
                                            90f,
                                            0,
                                            0,
                                            0,
                                            0,
                                            0,
                                            0);
        }

        private void OnPlayerRequestClass(PlayerRequestClassEvent eventdata)
        {
            eventdata.Player.Spawn();
        }

        private void OnPlayerConnect(PlayerConnectEvent eventdata)
        {
            eventdata.Player.HideDialogs();
        }
    }
}
