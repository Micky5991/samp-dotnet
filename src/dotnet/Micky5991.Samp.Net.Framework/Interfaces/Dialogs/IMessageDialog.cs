using System;
using System.Drawing;

namespace Micky5991.Samp.Net.Framework.Interfaces.Dialogs
{
    /// <summary>
    /// Builds a simple message dialog without input.
    /// </summary>
    public interface IMessageDialog : IDialog
    {
        /// <summary>
        /// Gets the current message of this dialog.
        /// </summary>
        string Message { get; }

        /// <summary>
        /// Changes the content message of the dialog.
        /// </summary>
        /// <param name="text">Text to display in the dialog.</param>
        /// <exception cref="ArgumentException"><paramref name="text"/> exceeds 4096 characters.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="text"/> is null.</exception>
        void SetMessage(string text);

        /// <summary>
        /// Changes the content message of the dialog.
        /// </summary>
        /// <param name="text">Text to display in the dialog.</param>
        /// <param name="color">Color of the message.</param>
        void SetMessage(string text, Color color);
    }
}
