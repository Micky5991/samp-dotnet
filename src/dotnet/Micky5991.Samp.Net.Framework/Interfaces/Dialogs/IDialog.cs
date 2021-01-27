using System;
using System.Drawing;
using Micky5991.Samp.Net.Framework.Data;

namespace Micky5991.Samp.Net.Framework.Interfaces.Dialogs
{
    /// <summary>
    /// Defines a buildable dialog with a caption.
    /// </summary>
    public interface IDialog
    {
        /// <summary>
        /// Delegate that will be used to add a colored element.
        /// </summary>
        /// <param name="title">Title of the element.</param>
        /// <param name="color">Color of the title.</param>
        public delegate void AddColoredEntryDelegate(string title, Color? color = null);

        /// <summary>
        /// Delegate that will be used to build the text on the dialog.
        /// </summary>
        /// <param name="leftButton">Action to set text on left button.</param>
        /// <param name="rightButton">Action to set text on right button.</param>
        public delegate void SetButtonsDelegate(
            AddColoredEntryDelegate leftButton,
            AddColoredEntryDelegate rightButton);

        /// <summary>
        /// Gets the current caption of this dialog.
        /// </summary>
        string Caption { get; }

        /// <summary>
        /// Gets the current text on the left button.
        /// </summary>
        string LeftButton { get; }

        /// <summary>
        /// Gets the current text on the right button.
        /// </summary>
        string RightButton { get; }

        /// <summary>
        /// Sets the caption of this dialog visible as header.
        /// </summary>
        /// <param name="caption">New value of the caption.</param>
        /// <exception cref="ArgumentNullException"><paramref name="caption"/> is null.</exception>
        void SetCaption(string caption);

        /// <summary>
        /// Sets the text on each button with this factory.
        /// </summary>
        /// <param name="factory">Factory that should be used to build the buttons.</param>
        /// <exception cref="ArgumentNullException"><paramref name="factory"/> is null.</exception>
        void SetButtons(SetButtonsDelegate factory);

        /// <summary>
        /// Sets the text on each button with this factory.
        /// </summary>
        /// <param name="leftText">Text on the left button. Supports colors.</param>
        /// <param name="rightText">Text on the right button. Supports colors.</param>
        /// <exception cref="ArgumentNullException"><paramref name="leftText"/> or <paramref name="rightText"/> is null.</exception>
        void SetButtons(string leftText, string rightText = "");

        /// <summary>
        /// Builds the dialog to display as <see cref="DialogData"/>.
        /// </summary>
        /// <returns>Resulting dialog data.</returns>
        DialogData Build();
    }
}
