using System.Threading.Tasks;
using Micky5991.Samp.Net.Framework.Data;
using Micky5991.Samp.Net.Framework.Interfaces.Dialogs;
using Micky5991.Samp.Net.Framework.Interfaces.Entities;

namespace Micky5991.Samp.Net.Framework.Interfaces.Services
{
    /// <summary>
    /// Handler that simplifies request and response of a dialog.
    /// </summary>
    public interface IDialogHandler : IEventListener
    {
        /// <summary>
        /// Shows a dialog to a player and waits for a response.
        /// </summary>
        /// <param name="player">Player to show the dialog to.</param>
        /// <param name="dialog">Dialog to display.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation and returning the result of this dialog.</returns>
        Task<DialogResponseData> ShowDialogAsync(IPlayer player, IDialog dialog);
    }
}
