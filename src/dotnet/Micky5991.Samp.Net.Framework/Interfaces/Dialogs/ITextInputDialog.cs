namespace Micky5991.Samp.Net.Framework.Interfaces.Dialogs
{
    /// <summary>
    /// Builds a dialog with an input.
    /// </summary>
    public interface ITextInputDialog : IMessageDialog
    {
        /// <summary>
        /// Gets a value indicating whether this text input dialog is masked.
        /// </summary>
        public bool Masked { get; }

        /// <summary>
        /// Sets the type of input of this dialog. This will change the style of this dialog to a password dialog.
        /// </summary>
        /// <param name="masked">true if the input should not be visible, false otherwise.</param>
        void SetMasked(bool masked = true);
    }
}
