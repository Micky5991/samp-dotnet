using System.Drawing;
using Micky5991.Samp.Net.Core.Natives.Samp;
using Micky5991.Samp.Net.Framework.Data;
using Micky5991.Samp.Net.Framework.Extensions;
using Micky5991.Samp.Net.Framework.Interfaces.Dialogs;

namespace Micky5991.Samp.Net.Framework.Elements.Dialogs
{
    /// <inheritdoc cref="IMessageDialog" />
    public class MessageDialog : Dialog, IMessageDialog
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MessageDialog"/> class.
        /// </summary>
        public MessageDialog()
        {
            this.Message = string.Empty;
        }

        /// <inheritdoc />
        public string Message { get; private set; }

        /// <inheritdoc />
        public void SetMessage(string text)
        {
            this.Message = text;
        }

        /// <inheritdoc />
        public void SetMessage(string text, Color color)
        {
            this.SetMessage($"{{{color.Embed()}}}{text}");
        }

        /// <inheritdoc />
        public override DialogData Build()
        {
            return new (DialogStyle.Msgbox, this.Caption, this.Message, this.LeftButton, this.RightButton);
        }
    }
}
