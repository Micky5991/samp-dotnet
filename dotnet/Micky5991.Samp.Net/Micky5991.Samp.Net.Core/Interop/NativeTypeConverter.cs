using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Micky5991.Samp.Net.Core.Interop
{
    public unsafe class NativeTypeConverter
    {
        private delegate IntPtr NativeWriteDelegate(object value, int size);
        private delegate object NativeReadDelegate(IntPtr value, int size);

        private readonly Dictionary<Type, (NativeWriteDelegate Writer, NativeReadDelegate Reader)> converters;

        public NativeTypeConverter()
        {
            this.converters = new Dictionary<Type, (NativeWriteDelegate Writer, NativeReadDelegate Reader)>
            {
                {typeof(int), ((x, y) => this.ConvertTypeToNative((int) x, y), this.ConvertNativeToInt)},
                {typeof(bool), ((x, y) => this.ConvertTypeToNative((bool) x, y), this.ConvertNativeToBool)},
                {typeof(float), ((x, y) => this.ConvertTypeToNative((float) x, y), this.ConvertNativeToSingle)},
                {typeof(double), ((x, y) => this.ConvertTypeToNative(Convert.ToSingle((double) x), y), (x, y) => Convert.ToDouble(this.ConvertNativeToSingle(x, y)))},
                {typeof(IntPtr), ((x, y) => this.ConvertTypeToNative((IntPtr) x, y), this.ConvertNativeToPointer)},
                {typeof(string), ((x, y) => this.ConvertTypeToNative((string) x, y), this.ConvertNativeToString)},
                // {typeof(byte[]), ((x, y) => this.ConvertTypeToNative((byte[]) x), x => this.ConvertNativeToBytes(x))},
            };
        }

        public (IntPtr arguments, Func<object>[] elements) BuildNativeArgumentPointer((object Value, int Size)[] arguments)
        {
            var elements = new Func<object>[arguments.Length];
            var location = Marshal.AllocHGlobal(sizeof(IntPtr) * arguments.Length);

            for (var i = 0; i < arguments.Length; i++)
            {
                var (success, argumentLocation, reader) = this.ConvertObjectToNative(arguments[i].Value, arguments[i].Size);
                if (success == false)
                {
                    return default;
                }

                elements[i] = reader;

                Marshal.WriteIntPtr(location, i * sizeof(IntPtr), argumentLocation);
            }

            return (location, elements);
        }

        public object[]? ReadPassedArgumentsAfterNative(Func<object>[] elements)
        {
            var arguments = new object[elements.Length];

            for (var i = 0; i < elements.Length; i++)
            {
                var value = elements[i]();
                if (value == null)
                {
                    return default;
                }

                arguments[i] = value;
            }

            return arguments;
        }

        public (bool Success, IntPtr Value, Func<object> Reader) ConvertObjectToNative(object value, int size)
        {
            if (this.converters.TryGetValue(value.GetType(), out var converter) == false)
            {
                return (false, IntPtr.Zero, null);
            }

            var location = converter.Writer(value, size);

            return (true, location, () => converter.Reader(location, size));
        }

        public IntPtr ConvertTypeToNative(int value, int size)
        {
            return this.ConvertTypeToNative(BitConverter.GetBytes(value));
        }

        public object ConvertNativeToInt(IntPtr location, int size)
        {
            var buffer = this.ConvertNativeToBytes(location, size);

            return BitConverter.ToInt32(buffer, 0);
        }

        public IntPtr ConvertTypeToNative(float value, int size)
        {
            return this.ConvertTypeToNative(BitConverter.GetBytes(value));
        }

        public object ConvertNativeToSingle(IntPtr location, int size)
        {
            var buffer = this.ConvertNativeToBytes(location, size);

            return BitConverter.ToSingle(buffer, 0);
        }

        public IntPtr ConvertTypeToNative(bool value, int size)
        {
            // Explicitly use single byte bool, because C# bool is 4 bytes long instead of 1 byte
            byte[] buffer =
            {
                value ? (byte) 1 : (byte) 0
            };

            return this.ConvertTypeToNative(buffer);
        }

        public object ConvertNativeToBool(IntPtr location, int size)
        {
            var buffer = this.ConvertNativeToBytes(location, 1);

            return buffer[0] == 1;
        }

        public IntPtr ConvertTypeToNative(IntPtr value, int size)
        {
            var location = Marshal.AllocHGlobal(size);

            Marshal.WriteIntPtr(location, 0, value);

            return location;
        }

        public object ConvertNativeToPointer(IntPtr location, int size)
        {
            var result = Marshal.ReadIntPtr(location);

            Marshal.FreeHGlobal(location);

            return result;
        }

        public IntPtr ConvertTypeToNative(string value, int size)
        {
            var buffer = new byte[size];
            Encoding.ASCII.GetBytes(value, 0, value.Length, buffer, 0);

            return this.ConvertTypeToNative(buffer);
        }

        public object ConvertNativeToString(IntPtr location, int size)
        {
            var buffer = this.ConvertNativeToBytes(location, size);

            return Encoding.ASCII.GetString(buffer);
        }

        public IntPtr ConvertTypeToNative(byte[] buffer)
        {
            var location = Marshal.AllocHGlobal(buffer.Length);
            Marshal.Copy(buffer, 0, location, buffer.Length);

            return location;
        }

        public byte[] ConvertNativeToBytes(IntPtr location, int size)
        {
            var buffer = new byte[size];

            Marshal.Copy(location, buffer, 0, buffer.Length);
            Marshal.FreeHGlobal(location);

            return buffer;
        }
    }
}
