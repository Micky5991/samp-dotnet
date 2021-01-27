using Micky5991.Samp.Net.Framework.Enums;

namespace Micky5991.Samp.Net.Framework.Data
{
    /// <summary>
    /// Stores data about a specific dialog response.
    /// </summary>
    public readonly struct DialogResponseData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DialogResponseData"/> struct.
        /// </summary>
        /// <param name="response">Sent dialog response.</param>
        /// <param name="listItem">Selected item in list, -1 if no list.</param>
        /// <param name="inputText">Entered text or selected list item.</param>
        public DialogResponseData(DialogResponse response, int listItem, string inputText)
        {
            this.Response = response;
            this.ListItem = listItem;
            this.InputText = inputText;
        }

        /// <summary>
        /// Gets the button that has been used for response.
        /// </summary>
        public DialogResponse Response { get; }

        /// <summary>
        /// Gets the index of the item that has been used. If no item has been selected or no item is available, -1.
        /// </summary>
        public int ListItem { get; }

        /// <summary>
        /// Gets the text that has been put into the input field or gets the text of this list item.
        /// </summary>
        public string InputText { get; }
    }
}
