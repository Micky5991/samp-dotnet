using System.ComponentModel;
using System.Drawing;
using System.Numerics;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using Micky5991.Samp.Net.Framework.Enums;
using Micky5991.Samp.Net.Framework.Interfaces.TextDraws;

namespace Micky5991.Samp.Net.Framework.Elements.TextDraws
{
    /// <inheritdoc />
    public class TextDraw : ITextDraw
    {
        private TextAlignment textAlignment = TextAlignment.Left;

        private Color backgroundColor = Color.Gray;

        private Color boxColor = Color.Black;

        private Color textColor = Color.White;

        private TextFont textFont = TextFont.AharoniBold;

        private Vector2 letterSize = new (0.5f, 2f);

        private int outlineSize;

        private bool propotional = true;

        private int shadowSize = 2;

        private string text;

        private Vector2 textSize = Vector2.One;

        private bool useBox;

        private bool selectable;

        private int previewModel;

        private (Vector3 Rotation, float Zoom) previewRotation;

        private (int Color1, int Color2) previewVehicleColor;

        private bool visible;

        private Vector2 position;

        private bool global;

        /// <summary>
        /// Initializes a new instance of the <see cref="TextDraw"/> class.
        /// </summary>
        /// <param name="position">Default position of the textdraw on the screen.</param>
        /// <param name="text">Default text of the textdraw.</param>
        public TextDraw(Vector2 position, string text)
        {
            this.position = position;
            this.text = text;
        }

        /// <inheritdoc />
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <inheritdoc />
        public Vector2 Position
        {
            get => this.position;
            set
            {
                if (value == this.position)
                {
                    return;
                }

                this.position = value;
                this.OnPropertyChanged();
            }
        }

        /// <inheritdoc />
        public TextAlignment TextAlignment
        {
            get => this.textAlignment;
            set
            {
                if (value == this.TextAlignment)
                {
                    return;
                }

                this.textAlignment = value;
                this.OnPropertyChanged();
            }
        }

        /// <inheritdoc />
        public Color BackgroundColor
        {
            get => this.backgroundColor;
            set
            {
                if (value == this.backgroundColor)
                {
                    return;
                }

                this.backgroundColor = value;
                this.OnPropertyChanged();
            }
        }

        /// <inheritdoc />
        public Color BoxColor
        {
            get => this.boxColor;
            set
            {
                if (value == this.boxColor)
                {
                    return;
                }

                this.boxColor = value;
                this.OnPropertyChanged();
            }
        }

        /// <inheritdoc />
        public Color TextColor
        {
            get => this.textColor;
            set
            {
                if (value == this.textColor)
                {
                    return;
                }

                this.textColor = value;
                this.OnPropertyChanged();
            }
        }

        /// <inheritdoc />
        public TextFont TextFont
        {
            get => this.textFont;
            set
            {
                if (value == this.textFont)
                {
                    return;
                }

                this.textFont = value;
                this.OnPropertyChanged();
            }
        }

        /// <inheritdoc />
        public Vector2 LetterSize
        {
            get => this.letterSize;
            set
            {
                if (value == this.letterSize)
                {
                    return;
                }

                this.letterSize = value;
                this.OnPropertyChanged();
            }
        }

        /// <inheritdoc />
        public int OutlineSize
        {
            get => this.outlineSize;
            set
            {
                if (value == this.outlineSize)
                {
                    return;
                }

                this.outlineSize = value;
                this.OnPropertyChanged();
            }
        }

        /// <inheritdoc />
        public bool Propotional
        {
            get => this.propotional;
            set
            {
                if (value == this.propotional)
                {
                    return;
                }

                this.propotional = value;
                this.OnPropertyChanged();
            }
        }

        /// <inheritdoc />
        public int ShadowSize
        {
            get => this.shadowSize;
            set
            {
                if (value == this.shadowSize)
                {
                    return;
                }

                this.shadowSize = value;
                this.OnPropertyChanged();
            }
        }

        /// <inheritdoc />
        public string Text
        {
            get => this.text;
            set
            {
                if (value.Length > 1024)
                {
                    value = value.Substring(0, 1024);
                }

                if (value == this.text)
                {
                    return;
                }

                this.text = value;
                this.OnPropertyChanged();
            }
        }

        /// <inheritdoc />
        public Vector2 TextSize
        {
            get => this.textSize;
            set
            {
                if (value == this.textSize)
                {
                    return;
                }

                this.textSize = value;
                this.OnPropertyChanged();
            }
        }

        /// <inheritdoc />
        public bool UseBox
        {
            get => this.useBox;
            set
            {
                if (value == this.useBox)
                {
                    return;
                }

                this.useBox = value;
                this.OnPropertyChanged();
            }
        }

        /// <inheritdoc />
        public bool Selectable
        {
            get => this.selectable;
            set
            {
                if (value == this.selectable)
                {
                    return;
                }

                this.selectable = value;
                this.OnPropertyChanged();
            }
        }

        /// <inheritdoc />
        public int PreviewModel
        {
            get => this.previewModel;
            set
            {
                if (value == this.previewModel)
                {
                    return;
                }

                this.previewModel = value;
                this.OnPropertyChanged();
            }
        }

        /// <inheritdoc />
        public (Vector3 Rotation, float Zoom) PreviewRotation
        {
            get => this.previewRotation;
            set
            {
                if (value == this.previewRotation)
                {
                    return;
                }

                this.previewRotation = value;
                this.OnPropertyChanged();
            }
        }

        /// <inheritdoc />
        public (int Color1, int Color2) PreviewVehicleColor
        {
            get => this.previewVehicleColor;
            set
            {
                if (value == this.previewVehicleColor)
                {
                    return;
                }

                this.previewVehicleColor = value;
                this.OnPropertyChanged();
            }
        }

        /// <inheritdoc />
        public bool Visible
        {
            get => this.visible;
            set
            {
                if (value == this.visible)
                {
                    return;
                }

                this.visible = value;
                this.OnPropertyChanged();
            }
        }

        /// <inheritdoc />
        public bool Global
        {
            get => this.global;
            set
            {
                if (value == this.global)
                {
                    return;
                }

                this.global = value;
                this.OnPropertyChanged();
            }
        }

        /// <inheritdoc />
        public ITextDraw Clone()
        {
            return new TextDraw(this.position, this.text)
            {
                backgroundColor = this.backgroundColor,
                textAlignment = this.textAlignment,
                boxColor = this.boxColor,
                textColor = this.textColor,
                textFont = this.textFont,
                letterSize = this.letterSize,
                outlineSize = this.outlineSize,
                propotional = this.propotional,
                shadowSize = this.shadowSize,
                textSize = this.textSize,
                useBox = this.useBox,
                selectable = this.selectable,
                previewModel = this.previewModel,
                previewRotation = this.previewRotation,
                previewVehicleColor = this.previewVehicleColor,
            };
        }

        /// <inheritdoc />
        public void Destroy()
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Signals subscribers about changed properties.
        /// </summary>
        /// <param name="propertyName">Property that has been updated.</param>
        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged(
            [CallerMemberName] string? propertyName = null)
        {
            this.PropertyChanged?.Invoke(
                                         this,
                                         new PropertyChangedEventArgs(propertyName));
        }
    }
}
