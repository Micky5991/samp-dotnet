using Micky5991.Samp.Net.Core.Natives.Samp;
using Micky5991.Samp.Net.Framework.Data;
using Micky5991.Samp.Net.Framework.Interfaces.Dialogs;

namespace Micky5991.Samp.Net.Framework.Elements.Dialogs
{
    /// <inheritdoc cref="ITextInputDialog" />
    public class TextInputDialog : MessageDialog, ITextInputDialog
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TextInputDialog"/> class.
        /// </summary>
        public TextInputDialog()
        {
            // Empty
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TextInputDialog"/> class.
        /// </summary>
        /// <param name="caption">Caption to use for dialog.</param>
        /// <param name="message">Message to display in dialog.</param>
        /// <param name="leftButton">Text on left button on dialog.</param>
        /// <param name="rightButton">Text on right button on dialog.</param>
        /// <param name="masked">true if input should be masked, false otherwise.</param>
        public TextInputDialog(string caption, string message, string leftButton, string rightButton = "", bool masked = false)
            : base(message, caption, leftButton, rightButton)
        {
            this.SetMasked(masked);
        }

        /// <inheritdoc />
        public bool Masked { get; private set; }

        /// <inheritdoc />
        public void SetMasked(bool masked = true)
        {
            this.Masked = masked;
        }

        /// <inheritdoc />
        public override DialogData Build()
        {
            return new (DialogStyle.Input, this.Caption, this.Message, this.LeftButton, this.RightButton);
        }
    }
}
