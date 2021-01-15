using System;
using System.Runtime.InteropServices;
using Micky5991.Samp.Net.Core.Interfaces.Interop;

namespace Micky5991.Samp.Net.Core.Interop.Converters
{
    public abstract class BaseNativeTypeConverter : INativeTypeConverter
    {
        public abstract Type Type { get; }

        public abstract IntPtr WriteValue(object value, int size);

        public abstract object ReadValue(IntPtr location, int size);

        protected IntPtr WriteBytesToNative(byte[] buffer)
        {
            var location = Marshal.AllocHGlobal(buffer.Length);
            Marshal.Copy(buffer, 0, location, buffer.Length);

            return location;
        }

        protected byte[] ReadBytesFromNative(IntPtr location, int size)
        {
            var buffer = new byte[size];

            Marshal.Copy(location, buffer, 0, buffer.Length);
            Marshal.FreeHGlobal(location);

            return buffer;
        }
    }
}
