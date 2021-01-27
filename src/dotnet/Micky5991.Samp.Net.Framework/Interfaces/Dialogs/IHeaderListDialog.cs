using System;
using System.Collections.Immutable;

namespace Micky5991.Samp.Net.Framework.Interfaces.Dialogs
{
    /// <summary>
    /// Represents a list dialog with a header row and resetting columns.
    /// </summary>
    public interface IHeaderListDialog : IListDialog
    {
        /// <summary>
        /// Delegate that will be used to add multiple headers to this list.
        /// </summary>
        /// <param name="addHeader">Factory method that should be used to build.</param>
        public delegate void BuildHeadersDelegate(AddColoredEntryDelegate addHeader);

        /// <summary>
        /// Gets the current ordered list of headers of this list dialog.
        /// </summary>
        IImmutableList<string> Headers { get; }

        /// <summary>
        /// Defines the header for this list. Make sure to call this before <see cref="IListDialog.AddRow"/>.
        /// </summary>
        /// <param name="factory">Factory that defines the column headers.</param>
        /// <exception cref="ArgumentNullException"><paramref name="factory"/> is null.</exception>
        void SetHeaders(BuildHeadersDelegate factory);

        /// <summary>
        /// Defines the header for this list. Make sure to call this before <see cref="IListDialog.AddRow"/>.
        /// </summary>
        /// <param name="headers">List of headers to add.</param>
        /// <exception cref="ArgumentNullException"><paramref name="headers"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="headers"/> contains null.</exception>
        void SetHeaders(params string[] headers);
    }
}
