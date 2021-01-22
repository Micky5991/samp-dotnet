using System;
using System.Drawing;

namespace Micky5991.Samp.Net.Framework.Extensions
{
    /// <summary>
    /// Provides extensions, so <see cref="Color"/> can be used with SAMP.
    /// </summary>
    public static class ColorExtensions
    {
        /// <summary>
        /// Converts a <see cref="Color"/> to a RGBA integer value compatible with chat messages.
        /// </summary>
        /// <param name="color"><see cref="Color"/> to be converted.</param>
        /// <returns>Converted color value.</returns>
        public static int ToRgba(this Color color)
        {
            return (color.R << 24) + (color.G << 16) + (color.B << 8) + color.A;
        }

        /// <summary>
        /// Multiplies the transparency with the given factor. Will be clamped between 0 and 1.
        /// </summary>
        /// <param name="color">Color to be changed.</param>
        /// <param name="transparencyFactor">Transparency value that should be set.</param>
        /// <returns>New color with new alpha value.</returns>
        public static Color Transparentize(this Color color, float transparencyFactor)
        {
            transparencyFactor = Math.Max(0, Math.Min(1, transparencyFactor));

            return Color.FromArgb((int)(color.A * transparencyFactor), color);
        }
    }
}
