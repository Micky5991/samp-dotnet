using Micky5991.EventAggregator.Elements;
using Micky5991.Samp.Net.Framework.Enums;
using Micky5991.Samp.Net.Framework.Interfaces.Entities;

namespace Micky5991.Samp.Net.Framework.Events.Samp
{
    /// <summary>
    /// Event that will be triggered when the player answers to a dialog.
    /// </summary>
    public class PlayerDialogResponseEvent : EventBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PlayerDialogResponseEvent"/> class.
        /// </summary>
        /// <param name="player">Player that sent the response.</param>
        /// <param name="dialogId">Id of the dialog that has been sent.</param>
        /// <param name="response">Type of response of this dialog.</param>
        /// <param name="listItem">Zero-based list item selected by the <see cref="Player"/>. If the dialog style is not a list style, it will be -1).</param>
        /// <param name="inputText">Value of the dialog that has been entered or the text of the list item that has been selected.</param>
        public PlayerDialogResponseEvent(IPlayer player, int dialogId, DialogResponse response, int listItem, string inputText)
        {
            this.Player = player;
            this.DialogId = dialogId;
            this.Response = response;
            this.ListItem = listItem;
            this.InputText = inputText;
        }

        /// <summary>
        /// Gets the player that sent the response.
        /// </summary>
        public IPlayer Player { get; }

        /// <summary>
        /// Gets the id of the dialog that has been sent.
        /// </summary>
        public int DialogId { get; }

        /// <summary>
        /// Gets the type of response of this dialog.
        /// </summary>
        public DialogResponse Response { get; }

        /// <summary>
        /// Gets the zero-based list item selected by the <see cref="Player"/>. If the dialog style is not a list style, it will be -1).
        /// </summary>
        public int ListItem { get; }

        /// <summary>
        /// Gets the value of the dialog that has been entered or the text of the list item that has been selected.
        /// </summary>
        public string InputText { get; }
    }
}
