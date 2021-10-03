using System;
using System.Collections.Immutable;
using Dawn;
using Micky5991.Samp.Net.Core.Natives.Samp;
using Micky5991.Samp.Net.Framework.Constants;
using Micky5991.Samp.Net.Framework.Data;
using Micky5991.Samp.Net.Framework.Interfaces.Dialogs;

namespace Micky5991.Samp.Net.Framework.Elements.Dialogs
{
    /// <inheritdoc cref="IListDialog" />
    public class ListDialog : Dialog, IListDialog
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ListDialog"/> class.
        /// </summary>
        public ListDialog()
        {
            // Empty
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ListDialog"/> class.
        /// </summary>
        /// <param name="caption">Caption to use for dialog.</param>
        /// <param name="leftButton">Text on left button on dialog.</param>
        /// <param name="rightButton">Text on right button on dialog.</param>
        /// <param name="colorResetting">true if colors should be reset for each row and column.</param>
        public ListDialog(string caption, string leftButton, string rightButton = "", bool colorResetting = false)
            : base(caption, leftButton, rightButton)
        {
            this.SetColorResetting(colorResetting);
        }

        /// <inheritdoc />
        public bool ColorResetting { get; private set; }

        /// <inheritdoc />
        public IImmutableList<string> Rows { get; private set; } = Array.Empty<string>().ToImmutableList();

        /// <inheritdoc />
        public void SetColorResetting(bool enabled = true)
        {
            this.ColorResetting = enabled;
        }

        /// <inheritdoc />
        public virtual void AddRow(params string[] row)
        {
            Guard.Argument(row, nameof(row)).NotNull();

            this.Rows = this.Rows.Add(string.Join($"{DialogConstants.Tab}", row));
        }

        /// <inheritdoc />
        public override DialogData Build()
        {
            return new (
                       this.ColorResetting ? DialogStyle.Tablist : DialogStyle.List,
                       this.Caption,
                       string.Join($"{DialogConstants.NewLine}", this.Rows),
                       this.LeftButton,
                       this.RightButton);
        }
    }
}
