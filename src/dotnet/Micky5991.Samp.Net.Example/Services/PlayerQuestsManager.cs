using System.Collections.Immutable;
using System.Drawing;
using System.Numerics;
using Micky5991.EventAggregator.Interfaces;
using Micky5991.Quests.Enums;
using Micky5991.Quests.Interfaces.Nodes;
using Micky5991.Samp.Net.Example.Events.EventArgs;
using Micky5991.Samp.Net.Example.Player.Renderer;
using Micky5991.Samp.Net.Framework.Elements.TextDraws;
using Micky5991.Samp.Net.Framework.Enums;
using Micky5991.Samp.Net.Framework.Interfaces;
using Micky5991.Samp.Net.Framework.Interfaces.Entities;

namespace Micky5991.Samp.Net.Example.Services
{
    public class PlayerQuestsManager : IEntityListener
    {
        private readonly IEventAggregator eventAggregator;

        private const string PlayerQuestsKey = "QUESTS_LIST";
        private const string PlayerQuestRenderersKey = "QUESTS_RENDERER_LIST";

        private readonly IImmutableList<TextDraw> titleTextdraws;

        public PlayerQuestsManager(IEventAggregator eventAggregator)
        {
            this.eventAggregator = eventAggregator;

            this.titleTextdraws = ImmutableArray<TextDraw>.Empty.AddRange(new[]
            {
                new TextDraw(new Vector2(500, 110), "Quests")
                {
                    TextFont = TextFont.BeckettRegular,
                    LetterSize = new Vector2(0.45f, 1.8f),
                    OutlineSize = 0,
                    TextColor = Color.White,
                    BackgroundColor = Color.Black,
                    TextSize = new Vector2(620, 300),
                },
                new TextDraw(new Vector2(490, 120), "-")
                {
                    TextFont = TextFont.BeckettRegular,
                    LetterSize = new Vector2(10f, 1.8f),
                    OutlineSize = 0,
                    TextColor = Color.White,
                    BackgroundColor = Color.Black,
                    TextSize = new Vector2(550, 130),
                },
            });
        }

        public void Attach()
        {
        }

        public IImmutableList<IQuestRootNode> GetPlayerQuests(IPlayer player)
        {
            if (player.TryGetData(PlayerQuestsKey, out IImmutableList<IQuestRootNode> quests) == false)
            {
                return ImmutableArray<IQuestRootNode>.Empty;
            }

            return quests;
        }

        public void GivePlayerQuest(IPlayer player, IQuestRootNode quest)
        {
            var currentQuests = this.GetPlayerQuests(player);
            currentQuests = currentQuests.Add(quest);

            player.SetData(PlayerQuestsKey, currentQuests);
            quest.SetStatus(QuestStatus.Active);

            this.ShowQuestDisplay(player, currentQuests);
        }

        public void RefreshQuests(IPlayer player)
        {
            var currentQuests = this.GetPlayerQuests(player);

            this.ShowQuestDisplay(player, currentQuests);
        }

        private void HideQuestDisplay(IPlayer player)
        {
            if (!player.TryGetData<IImmutableList<QuestRenderer>>(PlayerQuestRenderersKey, out var existingRenderers))
            {
                return;
            }

            foreach (var existingRenderer in existingRenderers)
            {
                existingRenderer.Updated -= this.OnQuestUpdated;
                existingRenderer.Dispose();
            }

            foreach (var textDraw in this.titleTextdraws)
            {
                player.HideTextDraw(textDraw);
            }

            player.TryRemoveData<IImmutableList<QuestRenderer>>(PlayerQuestRenderersKey, out _);
        }

        private void ShowQuestDisplay(IPlayer player, IImmutableList<IQuestRootNode> quests)
        {
            this.HideQuestDisplay(player);

            foreach (var titleTextdraw in this.titleTextdraws)
            {
                player.ShowTextDraw(titleTextdraw);
            }

            var currentPosition = new Vector2(500, 135);
            var renderers = ImmutableList<QuestRenderer>.Empty;

            foreach (var quest in quests)
            {
                var renderer = new QuestRenderer(player, quest);
                renderer.ShowTextDraw(currentPosition, true, out currentPosition);

                renderer.Updated += this.OnQuestUpdated;

                renderers = renderers.Add(renderer);
            }

            player.SetData(PlayerQuestRenderersKey, renderers);
        }

        private void OnQuestUpdated(object sender, QuestUpdatedEventArgs args)
        {
            if (sender is not QuestRenderer renderer)
            {
                return;
            }

            this.RefreshQuests(renderer.Player);
        }
    }
}
