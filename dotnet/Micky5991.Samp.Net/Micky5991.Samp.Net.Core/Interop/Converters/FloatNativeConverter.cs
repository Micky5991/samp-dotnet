using System;

namespace Micky5991.Samp.Net.Core.Interop.Converters
{
    public class FloatNativeConverter : BaseNativeTypeConverter
    {
        public override Type Type { get; } = typeof(float);

        public override IntPtr WriteValue(object value, int size)
        {
            return this.WriteBytesToNative(BitConverter.GetBytes((float) value));
        }

        public override object ReadValue(IntPtr location, int size)
        {
            var buffer = this.ReadBytesFromNative(location, size);

            return BitConverter.ToSingle(buffer, 0);
        }
    }
}
