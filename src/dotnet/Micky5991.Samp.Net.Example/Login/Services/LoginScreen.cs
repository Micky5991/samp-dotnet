using System;
using System.Numerics;
using Micky5991.EventAggregator.Interfaces;
using Micky5991.Samp.Net.Framework.Events.Samp;
using Micky5991.Samp.Net.Framework.Interfaces;

namespace Micky5991.Samp.Net.Example.Login.Services
{
    public class LoginScreen : IEventListener
    {
        private readonly IEventAggregator eventAggregator;

        public LoginScreen(IEventAggregator eventAggregator)
        {
            this.eventAggregator = eventAggregator;
        }

        public void Attach()
        {
            this.eventAggregator.Subscribe<PlayerRequestClassEvent>(this.OnPlayerRequestClass);
            this.eventAggregator.Subscribe<PlayerSpawnEvent>(this.OnPlayerSpawn);
        }

        private void OnPlayerSpawn(PlayerSpawnEvent eventdata)
        {
            eventdata.Player.ToggleSpectating(false);
            eventdata.Player.SetCameraBehindPlayer();
        }

        private void OnPlayerRequestClass(PlayerRequestClassEvent eventdata)
        {
            var player = eventdata.Player;

            player.ToggleSpectating(true);

            player.InterpolateCameraLookAt(
                                           new Vector3(1478.8278f, -857.0445f, 67.4494f),
                                           new Vector3(1370.9197f, -874.5152f, 111.3898f),
                                           TimeSpan.FromMinutes(2));

            player.InterpolateCameraPosition(
                                             new Vector3(1479.5795f, -857.7101f, 67.2642f),
                                             new Vector3(1370.4663f, -875.4114f, 111.7797f),
                                             TimeSpan.FromMinutes(2));
        }
    }
}
