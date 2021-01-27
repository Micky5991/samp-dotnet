using System.Collections.Immutable;
using Dawn;
using Micky5991.Samp.Net.Core.Natives.Samp;
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
            this.ColorResetting = false;
            this.Rows = new string[0].ToImmutableList();
        }

        /// <inheritdoc />
        public bool ColorResetting { get; private set; }

        /// <inheritdoc />
        public IImmutableList<string> Rows { get; private set; }

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
