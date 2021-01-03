using System;
using System.Text;

namespace Micky5991.Samp.Net.Core.Interop.Converters
{
    public class StringNativeConverter : BaseNativeTypeConverter
    {
        public override Type Type { get; } = typeof(string);

        public override IntPtr WriteValue(object value, int size)
        {
            var text = (string) value;

            var buffer = new byte[size];
            Encoding.ASCII.GetBytes(text, 0, text.Length, buffer, 0);

            return this.WriteBytesToNative(buffer);
        }

        public override object ReadValue(IntPtr location, int size)
        {
            var buffer = this.ReadBytesFromNative(location, size);

            return Encoding.ASCII.GetString(buffer);
        }
    }
}
