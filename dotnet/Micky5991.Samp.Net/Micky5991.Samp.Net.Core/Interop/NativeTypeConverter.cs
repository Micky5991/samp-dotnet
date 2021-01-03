using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using JetBrains.Annotations;
using Micky5991.Samp.Net.Core.Interfaces.Interop;
using Micky5991.Samp.Net.Core.Interop.Converters;

namespace Micky5991.Samp.Net.Core.Interop
{
    public unsafe class NativeTypeConverter
    {
        private readonly Dictionary<Type, INativeTypeConverter> converters;

        public NativeTypeConverter()
        {
            this.converters = new Dictionary<Type, INativeTypeConverter>();

            this.AddConverter(new IntegerNativeConverter());
            this.AddConverter(new FloatNativeConverter());
            this.AddConverter(new BoolNativeConverter());
            this.AddConverter(new StringNativeConverter());
        }

        private void AddConverter(INativeTypeConverter converter)
        {
            this.converters[converter.Type] = converter;
        }

        [UsedImplicitly]
        public (IntPtr arguments, Func<object>[] elements) WriteNativeArguments((object Value, int Size)[] arguments)
        {
            var elements = new Func<object>[arguments.Length];
            var location = Marshal.AllocHGlobal(sizeof(IntPtr) * arguments.Length);

            for (var i = 0; i < arguments.Length; i++)
            {
                var (success, argumentLocation, reader) = this.ConvertObjectToNativeArgument(arguments[i].Value, arguments[i].Size);
                if (success == false)
                {
                    return default;
                }

                elements[i] = reader;

                Marshal.WriteIntPtr(location, i * sizeof(IntPtr), argumentLocation);
            }

            return (location, elements);
        }

        [UsedImplicitly]
        public object[]? ReadNativeArguments(Func<object>[] elements)
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

        public (bool Success, IntPtr Value, Func<object> Reader) ConvertObjectToNativeArgument(object value, int size)
        {
            if (this.converters.TryGetValue(value.GetType(), out var converter) == false)
            {
                return (false, IntPtr.Zero, null);
            }

            var location = converter.WriteValue(value, size);

            return (true, location, () => converter.ReadValue(location, size));
        }
    }
}
