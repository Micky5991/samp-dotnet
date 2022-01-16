using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text.RegularExpressions;
using Micky5991.Quests.Enums;
using Micky5991.Quests.Interfaces.Nodes;
using Micky5991.Samp.Net.Example.Events.EventArgs;
using Micky5991.Samp.Net.Framework.Elements.TextDraws;
using Micky5991.Samp.Net.Framework.Enums;
using Micky5991.Samp.Net.Framework.Interfaces.Entities;

namespace Micky5991.Samp.Net.Example.Player.Renderer
{
    public class QuestRenderer : IDisposable
    {
        private const int LineDistanceY = 10;
        private const int LineDistanceX = 7;

        public event EventHandler<QuestUpdatedEventArgs> Updated;

        public IPlayer Player { get; }

        private readonly IQuestNode questNode;

        private readonly IImmutableList<QuestRenderer> childRenderers = ImmutableList<QuestRenderer>.Empty;

        public TextDraw TextDraw { get; private set; }

        public IEnumerable<TextDraw> AllTextDraws => this.childRenderers
                                                         .SelectMany(x => x.AllTextDraws)
                                                         .Append(this.TextDraw);

        public QuestRenderer(IPlayer player, IQuestNode questNode)
        {
            this.Player = player;
            this.questNode = questNode;

            if (this.questNode is IQuestCompositeNode compositeNode)
            {
                this.childRenderers = compositeNode.ChildNodes
                                                   .Select(x => new QuestRenderer(this.Player, x))
                                                   .ToImmutableList();
            }
            else if (this.questNode is IQuestRootNode rootNode)
            {
                this.childRenderers = this.childRenderers.Add(new QuestRenderer(this.Player, rootNode.ChildNode));
            }

            foreach (var childRenderer in this.childRenderers)
            {
                childRenderer.Updated += this.OnChildUpdated;
            }

            this.questNode.PropertyChanged += this.OnPropertyChanged;
        }

        private void OnChildUpdated(object sender, QuestUpdatedEventArgs args)
        {
            this.Updated?.Invoke(this, args);
        }

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(IQuestNode.Status):
                case nameof(IQuestNode.Title):
                    this.Updated?.Invoke(this, new QuestUpdatedEventArgs(sender as IQuestRootNode));

                    break;
            }
        }

        private string BuildTitle()
        {
            string title;

            switch (this.questNode)
            {
                case IQuestTaskNode task:
                    title = $"({(this.questNode.Status == QuestStatus.Success ? "X" : "   ")}) {task.Title}";

                    break;

                default:
                    title = this.questNode.Title;

                    break;
            }

            switch (this.questNode.Status)
            {
                case QuestStatus.Active:
                    return title;

                default:
                    return Regex.Replace(title, "~\\S~", "");
            }
        }

        private Color GetLineColor()
        {
            return this.questNode.Status switch
            {
                QuestStatus.Active => Color.White,
                QuestStatus.Success => Color.DarkOliveGreen,
                _ => Color.Gray
            };
        }

        public void ShowTextDraw(Vector2 origin, bool largeDisplay, out Vector2 newPosition)
        {
            if (largeDisplay)
            {
                this.TextDraw = new TextDraw(origin, this.BuildTitle())
                {
                    TextFont = TextFont.BankGothic,
                    LetterSize = new Vector2(0.25f, 1.13f),
                    OutlineSize = 0,
                    TextColor = Color.DodgerBlue,
                    BackgroundColor = Color.Black,
                    ShadowSize = 1,
                    TextSize = origin + new Vector2(150, 10),
                };
            }
            else
            {
                this.TextDraw = new TextDraw(origin, this.BuildTitle())
                {
                    TextFont = TextFont.AharoniBold,
                    LetterSize = new Vector2(0.2f, 0.9f),
                    OutlineSize = 0,
                    TextColor = this.GetLineColor(),
                    BackgroundColor = Color.Black,
                    ShadowSize = 1,
                    TextSize = origin + new Vector2(150, 10),
                };
            }

            this.Player.ShowTextDraw(this.TextDraw);

            newPosition = origin + new Vector2(0, LineDistanceY);

            var childOrigin = newPosition + new Vector2(LineDistanceX, 0);
            foreach (var childRenderer in this.childRenderers)
            {
                childRenderer.ShowTextDraw(childOrigin, false, out var newChildOrigin);

                // To accomodate to new vertical distance needed, add Y
                newPosition.Y = newChildOrigin.Y;
                childOrigin.Y = newChildOrigin.Y;
            }
        }

        public void HideTextDraw()
        {
            if (this.TextDraw != null)
            {
                this.Player.HideTextDraw(this.TextDraw);

                this.TextDraw = null;
            }

            foreach (var childRenderer in this.childRenderers)
            {
                childRenderer.HideTextDraw();
            }
        }

        public void Dispose()
        {
            this.HideTextDraw();

            this.questNode.PropertyChanged -= this.OnPropertyChanged;
            this.Updated = null;

            foreach (var childRenderer in this.childRenderers)
            {
                childRenderer.Dispose();
            }
        }
    }
}
