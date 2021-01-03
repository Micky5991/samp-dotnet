using System;

namespace Micky5991.Samp.Net.Core.Interop.Converters
{
    public class BoolNativeConverter : BaseNativeTypeConverter
    {
        public override Type Type { get; } = typeof(bool);

        public override IntPtr WriteValue(object value, int size)
        {
            // Explicitly use single byte bool, because C# bool is 4 bytes long instead of 1 byte
            byte[] buffer =
            {
                (bool) value ? (byte) 1 : (byte) 0
            };

            return this.WriteBytesToNative(buffer);
        }

        public override object ReadValue(IntPtr location, int size)
        {
            var buffer = this.ReadBytesFromNative(location, 1);

            return buffer[0] == 1;
        }
    }
}
