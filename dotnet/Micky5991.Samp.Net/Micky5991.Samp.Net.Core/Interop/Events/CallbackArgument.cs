using System;
using System.Runtime.InteropServices;

namespace Micky5991.Samp.Net.Core.Interop.Events
{
    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    public struct CallbackArgument
    {
        [FieldOffset(0)]
        public CallbackArgumentType Type;

        [FieldOffset(sizeof(CallbackArgumentType))]
        public int IntValue;

        [FieldOffset(sizeof(CallbackArgumentType))]
        public int BoolValue;

        [FieldOffset(sizeof(CallbackArgumentType))]
        public float FloatValue;

        [FieldOffset(sizeof(CallbackArgumentType))]
        public IntPtr PointerValue;
    }
}
