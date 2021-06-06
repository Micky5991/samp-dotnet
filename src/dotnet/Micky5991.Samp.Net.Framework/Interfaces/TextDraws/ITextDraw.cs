using System;
using System.ComponentModel;
using System.Drawing;
using System.Numerics;
using Micky5991.Samp.Net.Framework.Enums;

namespace Micky5991.Samp.Net.Framework.Interfaces.TextDraws
{
    /// <summary>
    /// Displays on the players screen. Changes to this instance are applied instantly.
    /// </summary>
    public interface ITextDraw : INotifyPropertyChanged
    {
        /// <summary>
        /// Gets or sets the position of this textdraw on the screen.
        /// </summary>
        public Vector2 Position { get; set; }

        /// <summary>
        /// Gets or sets the alignment of the text inside the textdraw.
        /// Left = Default.
        /// </summary>
        public TextAlignment TextAlignment { get; set; }

        /// <summary>
        /// Gets or sets the color of the background.
        /// </summary>
        public Color BackgroundColor { get; set; }

        /// <summary>
        /// Gets or sets the color of the textdraw box.
        /// </summary>
        public Color BoxColor { get; set; }

        /// <summary>
        /// Gets or sets the color of the textdraw text.
        /// </summary>
        public Color TextColor { get; set; }

        /// <summary>
        /// Gets or sets the font of the textdraw text.
        /// AharoniBold = Default.
        /// </summary>
        public TextFont TextFont { get; set; }

        /// <summary>
        /// Gets or sets the letter size of the textdraw text.
        /// </summary>
        public Vector2 LetterSize { get; set; }

        /// <summary>
        /// Gets or sets the outline thickness of the textdraw. 0 for no outline.
        /// </summary>
        public int OutlineSize { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the text spacing should be scaled to a propotional ratio. Useful when using <see cref="LetterSize"/> to ensure the text has even character spacing.
        /// true = Default.
        /// </summary>
        public bool Propotional { get; set; }

        /// <summary>
        /// Gets or sets the shadow size of the texttdraw. 1 = normal size shadow, 0 for no shadow.
        /// 2 = Default.
        /// </summary>
        public int ShadowSize { get; set; }

        /// <summary>
        /// Gets or sets the content of the textdraw. Limited to 1024 characters.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Gets or sets the size of the textdraw.
        /// </summary>
        public Vector2 TextSize { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the textdraw should show a box.
        /// </summary>
        public bool UseBox { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the textdraw is clickable.
        /// </summary>
        public bool Selectable { get; set; }

        /// <summary>
        /// Gets or sets the model that should be visible. Needs <see cref="TextFont"/> set to "PreviewModel".
        /// </summary>
        public int PreviewModel { get; set; }

        /// <summary>
        /// Gets or sets the model rotation inside the preview model.
        /// Smaller zoom value shows the model smaller, higher will make the model bigger.
        /// </summary>
        public (Vector3 Rotation, float Zoom) PreviewRotation { get; set; }

        /// <summary>
        /// Gets or sets the colors of the model if it is a vehicle.
        /// </summary>
        public (int Color1, int Color2) PreviewVehicleColor { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the textdraw is visible to anyone.
        /// </summary>
        public bool Visible { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the textdraw is should be global.
        /// </summary>
        public bool Global { get; set; }

        /// <summary>
        /// Clones the current textdraw instance with all their settings, without affecting other textdraws.
        /// </summary>
        /// <returns>Copied textdraw.</returns>
        public ITextDraw Clone();

        /// <summary>
        /// Destroys all currently visible instances of this textdraw.
        /// </summary>
        public void Destroy();
    }
}
