using System;
using System.Collections.Immutable;

namespace Micky5991.Samp.Net.Framework.Interfaces.Dialogs
{
    /// <summary>
    /// Describes a dialog that shows a standard list.
    /// </summary>
    public interface IListDialog : IDialog
    {
        /// <summary>
        /// Gets a value indicating whether the color will be reset for each new column and row.
        /// </summary>
        bool ColorResetting { get; }

        /// <summary>
        /// Gets the list of rows.
        /// </summary>
        IImmutableList<string> Rows { get; }

        /// <summary>
        /// Enables the reset of the text color if \n or \t has been used.
        /// </summary>
        /// <param name="enabled">true if colors should be reset.</param>
        void SetColorResetting(bool enabled = true);

        /// <summary>
        /// Adds a row to the list.
        /// </summary>
        /// <param name="row">Text that should be displayed on the list.</param>
        /// <exception cref="ArgumentException"><paramref name="row"/> is empty or has invalid length.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="row"/> is null.</exception>
        void AddRow(params string[] row);
    }
}
