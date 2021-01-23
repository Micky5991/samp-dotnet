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
        /// Converts a <see cref="Color"/> to a RGB integer value compatible with embedding in chat messages.
        /// </summary>
        /// <param name="color"><see cref="Color"/> to be converted.</param>
        /// <returns>Converted color value.</returns>
        public static int ToRgb(this Color color)
        {
            return (color.R << 16) + (color.G << 8) + color.B;
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

        /// <summary>
        /// Returns a string that can be used to embed color in strings.
        /// </summary>
        /// <param name="color">Color to format and return.</param>
        /// <returns>Embeddable color string for chat messages.</returns>
        public static string Embed(this Color color)
        {
            return $"{{{color.ToRgb():X6}}}";
        }
    }
}
