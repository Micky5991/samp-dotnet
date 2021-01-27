using System.Collections.Immutable;
using System.Drawing;
using Dawn;
using Micky5991.Samp.Net.Core.Natives.Samp;
using Micky5991.Samp.Net.Framework.Data;
using Micky5991.Samp.Net.Framework.Extensions;
using Micky5991.Samp.Net.Framework.Interfaces.Dialogs;

namespace Micky5991.Samp.Net.Framework.Elements.Dialogs
{
    /// <inheritdoc cref="IHeaderListDialog" />
    public class HeaderListDialog : ListDialog, IHeaderListDialog
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HeaderListDialog"/> class.
        /// </summary>
        public HeaderListDialog()
        {
            this.Headers = new string[0].ToImmutableList();
        }

        /// <inheritdoc />
        public IImmutableList<string> Headers { get; private set; }

        /// <inheritdoc />
        public void SetHeaders(IHeaderListDialog.BuildHeadersDelegate factory)
        {
            Guard.Argument(factory, nameof(factory)).NotNull();

            void HeaderFactory(string title, Color? color = null)
            {
                if (color == null)
                {
                    this.Headers = this.Headers.Add(title);
                }
                else
                {
                    this.Headers = this.Headers.Add($"{color.Value.Embed()}{title}");
                }
            }

            factory(HeaderFactory);
        }

        /// <inheritdoc />
        public override void AddRow(params string[] row)
        {
            Guard.Argument(row, nameof(row)).Count(this.Headers.Count);

            base.AddRow(row);
        }

        /// <inheritdoc />
        public void SetHeaders(params string[] headers)
        {
            Guard.Argument(headers, nameof(headers)).NotNull().DoesNotContainNull();

            this.SetHeaders(
                            x =>
                            {
                                x(string.Join($"{DialogConstants.Tab}", headers));
                            });
        }

        /// <inheritdoc />
        public override DialogData Build()
        {
            var headers = string.Join($"{DialogConstants.Tab}", this.Headers);
            var rows = string.Join($"{DialogConstants.NewLine}", this.Rows);

            return new (
                        DialogStyle.TablistHeaders,
                        this.Caption,
                        string.Join($"{DialogConstants.NewLine}", new { headers, rows, }),
                        this.LeftButton,
                        this.RightButton);
        }
    }
}
