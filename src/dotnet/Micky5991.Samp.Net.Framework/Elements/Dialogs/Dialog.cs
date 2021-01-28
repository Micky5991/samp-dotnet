using System.Drawing;
using Dawn;
using Micky5991.Samp.Net.Framework.Data;
using Micky5991.Samp.Net.Framework.Extensions;
using Micky5991.Samp.Net.Framework.Interfaces.Dialogs;

namespace Micky5991.Samp.Net.Framework.Elements.Dialogs
{
    /// <inheritdoc />
    public abstract class Dialog : IDialog
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Dialog"/> class.
        /// </summary>
        protected Dialog()
        {
            this.Caption = string.Empty;
            this.LeftButton = string.Empty;
            this.RightButton = string.Empty;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Dialog"/> class.
        /// </summary>
        /// <param name="caption">Caption to use for dialog.</param>
        /// <param name="leftButton">Text on left button on dialog.</param>
        /// <param name="rightButton">Text on right button on dialog.</param>
        protected Dialog(string caption, string leftButton, string rightButton = "")
            : this()
        {
            this.SetCaption(caption);
            this.SetButtons(leftButton, rightButton);
        }

        /// <inheritdoc />
        public string Caption { get; private set; }

        /// <inheritdoc />
        public string LeftButton { get; private set; }

        /// <inheritdoc />
        public string RightButton { get; private set; }

        /// <inheritdoc />
        public void SetCaption(string caption)
        {
            Guard.Argument(caption, nameof(caption)).NotNull();

            this.Caption = caption;
        }

        /// <inheritdoc />
        public void SetButtons(IDialog.SetButtonsDelegate factory)
        {
            Guard.Argument(factory, nameof(factory)).NotNull();

            void BuildLeftButton(string title, Color? color = null)
            {
                Guard.Argument(title, nameof(title)).NotNull();

                color ??= Color.White;

                if (string.IsNullOrWhiteSpace(title))
                {
                    return;
                }

                this.LeftButton = $"{color.Value.Embed()}{title}";
            }

            void BuildRightButton(string title, Color? color = null)
            {
                Guard.Argument(title, nameof(title)).NotNull();

                color ??= Color.White;

                if (string.IsNullOrWhiteSpace(title))
                {
                    return;
                }

                this.RightButton = $"{color.Value.Embed()}{title}";
            }

            factory(BuildLeftButton, BuildRightButton);
        }

        /// <inheritdoc />
        public void SetButtons(string leftText, string rightText = "")
        {
            Guard.Argument(leftText, nameof(leftText)).NotNull();
            Guard.Argument(rightText, nameof(rightText)).NotNull();

            this.SetButtons(
                            (leftbutton, rightButton) =>
                            {
                                leftbutton(leftText);
                                rightButton(rightText);
                            });
        }

        /// <inheritdoc />
        public abstract DialogData Build();
    }
}
