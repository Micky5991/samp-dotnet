using System.Linq;
using System.Net;
using System.Security.Claims;
using Micky5991.EventAggregator.Interfaces;
using Micky5991.Samp.Net.Core.Natives.Samp;
using Micky5991.Samp.Net.Framework.Constants;
using Micky5991.Samp.Net.Framework.Interfaces;
using Micky5991.Samp.Net.Framework.Interfaces.Entities.Pools;
using Microsoft.Extensions.Logging;

namespace Micky5991.Samp.Net.Framework.Elements.Entities.Listeners
{
    /// <summary>
    /// Listener that subscribes to successful <see cref="NativeRconLoginAttemptEvent"/> events and attaches a role to
    /// the player.
    /// </summary>
    public class RconEventListeners : IEventListener
    {
        private readonly IEventAggregator eventAggregator;

        private readonly IPlayerPool playerPool;

        private readonly ILogger<RconEventListeners> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="RconEventListeners"/> class.
        /// </summary>
        /// <param name="eventAggregator">Eventaggregator where the needed event will be published.</param>
        /// <param name="playerPool">Pool where the player is registered in.</param>
        /// <param name="logger">Logger for warnings of this listener.</param>
        public RconEventListeners(IEventAggregator eventAggregator, IPlayerPool playerPool, ILogger<RconEventListeners> logger)
        {
            this.eventAggregator = eventAggregator;
            this.playerPool = playerPool;
            this.logger = logger;
        }

        /// <inheritdoc />
        public void Attach()
        {
            this.eventAggregator.Subscribe<NativeRconLoginAttemptEvent>(this.OnRconLogin);
        }

        private void OnRconLogin(NativeRconLoginAttemptEvent eventdata)
        {
            if (eventdata.Success == false)
            {
                return;
            }

            var player = this.playerPool.Entities.Values.FirstOrDefault(x => x.Ip.Equals(IPAddress.Parse(eventdata.Ip)));
            if (player == null)
            {
                return;
            }

            var claims = new[]
            {
                new Claim(SampClaimTypes.IpAddress, eventdata.Ip),
                new Claim(ClaimTypes.Role, "RconAdmin"),
            };

            var identity = new ClaimsIdentity(claims, "SAMP Rcon");

            player.Principal.AddIdentity(identity);
        }
    }
}
