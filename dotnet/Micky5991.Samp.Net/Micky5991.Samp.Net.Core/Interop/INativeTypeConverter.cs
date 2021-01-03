using System;

namespace Micky5991.Samp.Net.Core.Interop
{
    public interface INativeTypeConverter
    {

        public Type Type { get; }

        public IntPtr WriteValue(object value, int size);
        public object ReadValue(IntPtr location, int size);

    }
}
