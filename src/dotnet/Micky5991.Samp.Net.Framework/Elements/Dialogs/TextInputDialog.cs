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
            this.Masked = false;
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
