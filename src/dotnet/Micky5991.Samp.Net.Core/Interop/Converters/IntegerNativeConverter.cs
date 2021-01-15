using System;

namespace Micky5991.Samp.Net.Core.Interop.Converters
{
    public class IntegerNativeConverter : BaseNativeTypeConverter
    {
        public override Type Type { get; } = typeof(int);

        public override IntPtr WriteValue(object value, int size)
        {
            return this.WriteBytesToNative(BitConverter.GetBytes((int) value));
        }

        public override object ReadValue(IntPtr location, int size)
        {
            var buffer = this.ReadBytesFromNative(location, size);

            return BitConverter.ToInt32(buffer, 0);
        }
    }
}
