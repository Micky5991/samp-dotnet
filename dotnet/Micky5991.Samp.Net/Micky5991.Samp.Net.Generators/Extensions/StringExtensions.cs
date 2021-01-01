using System;
using System.Text;

namespace Micky5991.Samp.Net.Generators.Extensions
{
    internal static class StringExtensions
    {

        internal static string Indent(this string text, int indention)
        {
            var builder = new StringBuilder("".PadLeft(indention * 4));

            builder.Append(text);

            return builder.ToString();
        }

        internal static string ConvertToPascalCase(this string text)
        {
            text = text.ToLower();
            var builder = new StringBuilder();
            var upperChar = true;

            foreach (var c in text)
            {
                if (upperChar)
                {
                    builder.Append(char.ToUpper(c));
                    upperChar = false;

                    continue;
                }

                if (char.IsLetterOrDigit(c))
                {
                    builder.Append(c);

                    continue;
                }

                upperChar = true;
            }

            return builder.ToString();
        }

        internal static string ConvertToSnakeCase(this string text)
        {
            var builder = new StringBuilder();

            foreach (var c in text)
            {
                if (builder.Length == 0 || char.IsUpper(c) == false)
                {
                    builder.Append(char.ToLower(c));

                    continue;
                }

                builder.Append("_");
                builder.Append(char.ToLower(c));
            }

            return builder.ToString();
        }

        internal static string ConvertToCamelCase(this string text)
        {
            if (text.ToUpper() != text) // Text has special casing
            {
                text = text.ConvertToSnakeCase();
            }

            text = text.ToLower();
            var builder = new StringBuilder();
            var upperChar = false;

            foreach (var c in text)
            {
                if (upperChar)
                {
                    builder.Append(char.ToUpper(c));
                    upperChar = false;

                    continue;
                }

                if (char.IsLetterOrDigit(c))
                {
                    builder.Append(c);

                    continue;
                }

                upperChar = true;
            }

            return builder.ToString();
        }

    }
}
