using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Micky5991.Samp.Net.Core.Native
{
    public unsafe class NativeTypeConverter
    {
        private readonly Dictionary<Type, Func<object, IntPtr>> converters;

        public NativeTypeConverter()
        {
            this.converters = new Dictionary<Type, Func<object, IntPtr>>
            {
                {typeof(int), x => this.ConvertTypeToNative((int) x)},
                {typeof(bool), x => this.ConvertTypeToNative((bool) x)},
                {typeof(float), x => this.ConvertTypeToNative((float) x)},
                {typeof(double), x => this.ConvertTypeToNative(Convert.ToSingle((double) x))},
                {typeof(IntPtr), x => this.ConvertTypeToNative((IntPtr) x)},
                {typeof(string), x => this.ConvertTypeToNative((string) x)},
                {typeof(byte[]), x => this.ConvertTypeToNative((byte[]) x)},
            };
        }

        public (bool Success, IntPtr Value) ConvertTypeToNative(object value)
        {
            if (this.converters.TryGetValue(value.GetType(), out var converter) == false)
            {
                return (false, IntPtr.Zero);
            }

            return (true, converter(value));
        }

        private IntPtr ConvertTypeToNative(int value)
        {
            return this.ConvertTypeToNative(BitConverter.GetBytes(value));
        }

        private IntPtr ConvertTypeToNative(float value)
        {
            return this.ConvertTypeToNative(BitConverter.GetBytes(value));
        }

        private IntPtr ConvertTypeToNative(bool value)
        {
            // Explicitly use single byte bool, because C# bool is 4 bytes long instead of 1 byte
            byte[] buffer =
            {
                value ? (byte) 1 : (byte) 0
            };

            return this.ConvertTypeToNative(buffer);
        }

        private IntPtr ConvertTypeToNative(IntPtr value)
        {
            // Explicitly use single byte bool, because C# bool is 4 bytes long instead of 1 byte
            var location = Marshal.AllocHGlobal(sizeof(IntPtr));

            Marshal.WriteIntPtr(location, 0, value);

            return location;
        }

        private IntPtr ConvertTypeToNative(string value)
        {
            var buffer = new byte[value.Length + 1];
            Encoding.Default.GetBytes(value, 0, value.Length, buffer, 0);

            var location = Marshal.AllocHGlobal(buffer.Length);

            Marshal.Copy(buffer, 0, location, buffer.Length);

            return location;
        }

        private IntPtr ConvertTypeToNative(byte[] buffer)
        {
            var location = Marshal.AllocHGlobal(buffer.Length);
            Marshal.Copy(buffer, 0, location, buffer.Length);

            return location;
        }
    }
}
