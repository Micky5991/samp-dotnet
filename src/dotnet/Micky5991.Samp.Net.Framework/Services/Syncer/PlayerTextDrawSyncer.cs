using System.Collections.Generic;
using Micky5991.EventAggregator;
using Micky5991.EventAggregator.Interfaces;
using Micky5991.Samp.Net.Core.Natives.Players;
using Micky5991.Samp.Net.Framework.Elements.TextDraws;
using Micky5991.Samp.Net.Framework.Events;
using Micky5991.Samp.Net.Framework.Events.Samp;
using Micky5991.Samp.Net.Framework.Interfaces;
using Micky5991.Samp.Net.Framework.Interfaces.Entities;
using Micky5991.Samp.Net.Framework.Interfaces.TextDraws;
using Microsoft.Extensions.Logging;

namespace Micky5991.Samp.Net.Framework.Services.Syncer
{
    /// <summary>
    /// Syncer that keeps the <see cref="ITextDraw"/> instance in sync with SA:MP.
    /// </summary>
    public class PlayerTextDrawSyncer : IEventListener
    {
        private readonly IEventAggregator eventAggregator;

        private readonly IPlayersNatives playersNatives;

        private readonly ILogger<PlayerTextDrawBind> bindLogger;

        private readonly Dictionary<IPlayer, Dictionary<ITextDraw, PlayerTextDrawBind>> binds;

        /// <summary>
        /// Initializes a new instance of the <see cref="PlayerTextDrawSyncer"/> class.
        /// </summary>
        /// <param name="eventAggregator">Event aggregator to use.</param>
        /// <param name="playersNatives">Natives caller needed to update player textdraws.</param>
        /// <param name="bindLogger">Logger needed to log messages inside bind.</param>
        public PlayerTextDrawSyncer(IEventAggregator eventAggregator, IPlayersNatives playersNatives, ILogger<PlayerTextDrawBind> bindLogger)
        {
            this.eventAggregator = eventAggregator;
            this.playersNatives = playersNatives;
            this.bindLogger = bindLogger;

            this.binds = new Dictionary<IPlayer, Dictionary<ITextDraw, PlayerTextDrawBind>>();
        }

        /// <inheritdoc />
        public void Attach()
        {
            this.eventAggregator.Subscribe<PlayerShowTextDrawEvent>(
                                                                    this.OnShowTextDraw,
                                                                    threadTarget: ThreadTarget.MainThread);
            this.eventAggregator.Subscribe<PlayerHideTextDrawEvent>(
                                                                    this.OnHideTextDraw,
                                                                    threadTarget: ThreadTarget.MainThread);
            this.eventAggregator.Subscribe<PlayerDisconnectEvent>(
                                                                  this.OnPlayerDisconnect,
                                                                  threadTarget: ThreadTarget.PublisherThread);
        }

        private void OnPlayerDisconnect(PlayerDisconnectEvent eventdata)
        {
            var player = eventdata.Player;

            if (this.binds.TryGetValue(player, out var playerBinds) == false)
            {
                return;
            }

            foreach (var bind in playerBinds.Values)
            {
                if (bind.Disposed)
                {
                    continue;
                }

                bind.Dispose();
            }

            this.binds.Remove(player);
        }

        private void OnShowTextDraw(PlayerShowTextDrawEvent eventdata)
        {
            var player = eventdata.Player;
            var textDraw = eventdata.TextDraw;

            if (textDraw.Global)
            {
                return;
            }

            if (this.binds.TryGetValue(player, out var playerBinds) == false)
            {
                playerBinds = new Dictionary<ITextDraw, PlayerTextDrawBind>();

                this.binds.Add(player, playerBinds);
            }

            if (playerBinds.TryGetValue(textDraw, out var drawBind))
            {
                playerBinds.Remove(textDraw);
                drawBind.Dispose();
            }

            var bind = new PlayerTextDrawBind(
                                              player,
                                              textDraw,
                                              this.playersNatives,
                                              this.bindLogger);

            playerBinds.Add(textDraw, bind);

            bind.Build();
        }

        private void OnHideTextDraw(PlayerHideTextDrawEvent eventdata)
        {
            var player = eventdata.Player;
            var textDraw = eventdata.TextDraw;

            if (
                this.binds.TryGetValue(player, out var playerBinds) == false ||
                playerBinds.TryGetValue(textDraw, out var drawBind) == false)
            {
                return;
            }

            drawBind.Dispose();

            if (playerBinds.Count > 1)
            {
                playerBinds.Remove(textDraw);
            }
            else
            {
                this.binds.Remove(player);
            }
        }
    }
}
