using Micky5991.Samp.Net.Core.Natives.Samp;

namespace Micky5991.Samp.Net.Framework.Data
{
    /// <summary>
    /// Stores data about displaying a dialog to the player.
    /// </summary>
    public readonly struct DialogData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DialogData"/> struct.
        /// </summary>
        /// <param name="style">Style to display the dialog as.</param>
        /// <param name="caption">Caption to use to show in the boxes header.</param>
        /// <param name="info">Actual content to show inside dialog.</param>
        /// <param name="leftButton">Text on the left button.</param>
        /// <param name="rightButton">Optional text on the right button.</param>
        public DialogData(DialogStyle style, string caption, string info, string leftButton, string rightButton)
        {
            this.Style = style;
            this.Caption = caption;
            this.Info = info;
            this.LeftButton = leftButton;
            this.RightButton = rightButton;
        }

        /// <summary>
        /// Gets the style this dialog should be displayed as.
        /// </summary>
        public DialogStyle Style { get; }

        /// <summary>
        /// Gets the caption of this dialog.
        /// </summary>
        public string Caption { get; }

        /// <summary>
        /// Gets the content of this dialog. It will be formatted dependent of <see cref="Style"/>.
        /// </summary>
        public string Info { get; }

        /// <summary>
        /// Gets the text on the left button.
        /// </summary>
        public string LeftButton { get; }

        /// <summary>
        /// Gets the text on the right button of the dialog. Keep empty to hide it.
        /// </summary>
        public string RightButton { get; }
    }
}
