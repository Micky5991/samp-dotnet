using System;
using System.Collections.Concurrent;
using System.ComponentModel;
using Dawn;
using Micky5991.Samp.Net.Core.Natives.Players;
using Micky5991.Samp.Net.Framework.Extensions;
using Micky5991.Samp.Net.Framework.Interfaces.Entities;
using Micky5991.Samp.Net.Framework.Interfaces.TextDraws;
using Microsoft.Extensions.Logging;

namespace Micky5991.Samp.Net.Framework.Elements.TextDraws
{
    /// <summary>
    /// Binds <see cref="ITextDraw"/> to player and watches for updates.
    /// </summary>
    public class PlayerTextDrawBind : IDisposable
    {
        private readonly IPlayer player;

        private readonly ITextDraw textDraw;

        private readonly IPlayersNatives playersNatives;

        private readonly ILogger<PlayerTextDrawBind> logger;

        private readonly ConcurrentDictionary<string, Action> propertyUpdater;

        private int? id;

        /// <summary>
        /// Initializes a new instance of the <see cref="PlayerTextDrawBind"/> class.
        /// </summary>
        /// <param name="player">Player to show the textdraw to.</param>
        /// <param name="textDraw">Textdraw to show.</param>
        /// <param name="playersNatives">Natives to manage the player textdraw.</param>
        /// <param name="logger">Logger that shows any log messages.</param>
        public PlayerTextDrawBind(
            IPlayer player,
            ITextDraw textDraw,
            IPlayersNatives playersNatives,
            ILogger<PlayerTextDrawBind> logger)
        {
            this.player = player;
            this.textDraw = textDraw;
            this.playersNatives = playersNatives;
            this.logger = logger;

            this.textDraw.PropertyChanged += this.OnTextDrawChanged;

            this.propertyUpdater = new ConcurrentDictionary<string, Action>
            {
                [nameof(ITextDraw.TextAlignment)] = this.UpdateTextAlignment,
                [nameof(ITextDraw.TextColor)] = this.UpdateTextColor,
                [nameof(ITextDraw.TextFont)] = this.UpdateTextFont,
                [nameof(ITextDraw.ShadowSize)] = this.UpdateShadowSize,
                [nameof(ITextDraw.Propotional)] = this.UpdatePropotional,
                [nameof(ITextDraw.Selectable)] = this.UpdateSelectable,
                [nameof(ITextDraw.BackgroundColor)] = this.UpdateBackgroundColor,
                [nameof(ITextDraw.BoxColor)] = this.UpdateBoxColor,
                [nameof(ITextDraw.TextColor)] = this.UpdateTextColor,
                [nameof(ITextDraw.PreviewModel)] = this.UpdatePreviewModel,
                [nameof(ITextDraw.PreviewRotation)] = this.UpdatePreviewRotation,
                [nameof(ITextDraw.PreviewVehicleColor)] = this.UpdatePreviewVehicleColor,
                [nameof(ITextDraw.LetterSize)] = this.UpdateLetterSize,
                [nameof(ITextDraw.OutlineSize)] = this.UpdateOutlineSize,
                [nameof(ITextDraw.TextSize)] = this.UpdateTextSize,
                [nameof(ITextDraw.UseBox)] = this.UpdateUseBox,
                [nameof(ITextDraw.Text)] = this.UpdateText,
            };
        }

        /// <summary>
        /// Gets a value indicating whether the object has been disposed.
        /// </summary>
        public bool Disposed { get; private set; }

        /// <summary>
        /// Builds the textdraw for the specific player.
        /// </summary>
        /// <exception cref="ObjectDisposedException">Bind has been disposed.</exception>
        public void Build()
        {
            Guard.Disposal(this.Disposed);

            if (this.id != null)
            {
                return;
            }

            this.id = this.playersNatives.CreatePlayerTextDraw(
                                                               this.player.Id,
                                                               this.textDraw.Position.X,
                                                               this.textDraw.Position.Y,
                                                               this.textDraw.Text);

            foreach (var propertyUpdaterValue in this.propertyUpdater.Values)
            {
                propertyUpdaterValue();
            }

            this.Refresh();
        }

        /// <summary>
        /// Destroys the textdraw for the specific player. Can still be recreated.
        /// </summary>
        /// <exception cref="ObjectDisposedException">Bind has been disposed.</exception>
        public void Destroy()
        {
            Guard.Disposal(this.Disposed);

            if (this.id == null)
            {
                return;
            }

            this.playersNatives.PlayerTextDrawDestroy(
                                                      this.player.Id,
                                                      this.id.Value);

            this.id = null;
        }

        /// <inheritdoc />
        public void Dispose()
        {
            Guard.Disposal(this.Disposed);

            this.Destroy();

            this.Disposed = true;

            this.textDraw.PropertyChanged -= this.OnTextDrawChanged;
        }

        private void UpdateTextAlignment()
        {
            this.playersNatives.PlayerTextDrawAlignment(
                                                        this.player.Id,
                                                        this.id!.Value,
                                                        (int)this.textDraw.TextAlignment);
        }

        private void UpdateBackgroundColor()
        {
            this.playersNatives.PlayerTextDrawBackgroundColor(
                                                              this.player.Id,
                                                              this.id!.Value,
                                                              this.textDraw.BackgroundColor.ToRgba());
        }

        private void UpdateBoxColor()
        {
            this.playersNatives.PlayerTextDrawBoxColor(
                                                       this.player.Id,
                                                       this.id!.Value,
                                                       this.textDraw.BoxColor.ToRgba());
        }

        private void UpdateTextColor()
        {
            this.playersNatives.PlayerTextDrawColor(
                                                    this.player.Id,
                                                    this.id!.Value,
                                                    this.textDraw.TextColor.ToRgba());
        }

        private void UpdateTextFont()
        {
            this.playersNatives.PlayerTextDrawFont(
                                                   this.player.Id,
                                                   this.id!.Value,
                                                   (int)this.textDraw.TextFont);
        }

        private void UpdateLetterSize()
        {
            this.playersNatives.PlayerTextDrawLetterSize(
                                                         this.player.Id,
                                                         this.id!.Value,
                                                         this.textDraw.LetterSize.X,
                                                         this.textDraw.LetterSize.Y);
        }

        private void UpdateOutlineSize()
        {
            this.playersNatives.PlayerTextDrawSetOutline(
                                                         this.player.Id,
                                                         this.id!.Value,
                                                         this.textDraw.OutlineSize);
        }

        private void UpdatePropotional()
        {
            this.playersNatives.PlayerTextDrawSetProportional(
                                                              this.player.Id,
                                                              this.id!.Value,
                                                              this.textDraw.Propotional);
        }

        private void UpdateShadowSize()
        {
            this.playersNatives.PlayerTextDrawSetShadow(
                                                        this.player.Id,
                                                        this.id!.Value,
                                                        this.textDraw.ShadowSize);
        }

        private void UpdateText()
        {
            this.playersNatives.PlayerTextDrawSetString(
                                                        this.player.Id,
                                                        this.id!.Value,
                                                        this.textDraw.Text);
        }

        private void UpdateTextSize()
        {
            this.playersNatives.PlayerTextDrawTextSize(
                                                       this.player.Id,
                                                       this.id!.Value,
                                                       this.textDraw.TextSize.X,
                                                       this.textDraw.TextSize.Y);
        }

        private void UpdateUseBox()
        {
            this.playersNatives.PlayerTextDrawUseBox(
                                                     this.player.Id,
                                                     this.id!.Value,
                                                     this.textDraw.UseBox);
        }

        private void UpdateSelectable()
        {
            this.playersNatives.PlayerTextDrawSetSelectable(
                                                            this.player.Id,
                                                            this.id!.Value,
                                                            this.textDraw.Selectable);
        }

        private void UpdatePreviewModel()
        {
            this.playersNatives.PlayerTextDrawSetPreviewModel(
                                                              this.player.Id,
                                                              this.id!.Value,
                                                              this.textDraw.PreviewModel);
        }

        private void UpdatePreviewRotation()
        {
            var rotation = this.textDraw.PreviewRotation.Rotation;
            var zoom = this.textDraw.PreviewRotation.Zoom;

            this.playersNatives.PlayerTextDrawSetPreviewRot(
                                                            this.player.Id,
                                                            this.id!.Value,
                                                            rotation.X,
                                                            rotation.Y,
                                                            rotation.Z,
                                                            zoom);
        }

        private void UpdatePreviewVehicleColor()
        {
            this.playersNatives.PlayerTextDrawSetPreviewVehCol(
                                                            this.player.Id,
                                                            this.id!.Value,
                                                            this.textDraw.PreviewVehicleColor.Color1,
                                                            this.textDraw.PreviewVehicleColor.Color2);
        }

        private void OnTextDrawChanged(
            object sender,
            PropertyChangedEventArgs e)
        {
            this.logger.LogTrace($"Update texdraw: {e.PropertyName} - {this.id}");

            switch (e.PropertyName)
            {
                case nameof(ITextDraw.Position):
                    this.Destroy();
                    this.Build();

                    break;

                default:
                    if (this.propertyUpdater.TryGetValue(e.PropertyName, out var updater))
                    {
                        updater();
                        this.Refresh();
                    }

                    break;
            }
        }

        private void Refresh()
        {
            if (this.id == null)
            {
                this.logger.LogWarning($"Tried to refresh unknown textdraw to ${this.player}");

                return;
            }

            this.playersNatives.PlayerTextDrawShow(
                                                   this.player.Id,
                                                   this.id.Value);
        }
    }
}
