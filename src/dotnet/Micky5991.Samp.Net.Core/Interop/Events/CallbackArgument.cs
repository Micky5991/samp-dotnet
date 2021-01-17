using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Micky5991.Samp.Net.Core.Interop.Events
{
    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    public struct CallbackArgument
    {
        [FieldOffset(0)]
        public CallbackArgumentType ValueType;

        [FieldOffset(sizeof(CallbackArgumentType))]
        public int Size;

        [FieldOffset(sizeof(CallbackArgumentType) + sizeof(int))]
        public int IntValue;

        [FieldOffset(sizeof(CallbackArgumentType) + sizeof(int))]
        [MarshalAs(UnmanagedType.U1)]
        public bool BoolValue;

        [FieldOffset(sizeof(CallbackArgumentType) + sizeof(int))]
        public float FloatValue;

        [FieldOffset(sizeof(CallbackArgumentType) + sizeof(int))]
        public IntPtr PointerValue;

        public object GetValue()
        {
            switch (this.ValueType)
            {
                case CallbackArgumentType.Integer:
                    return this.IntValue;

                case CallbackArgumentType.Float:
                    return this.FloatValue;

                case CallbackArgumentType.Bool:
                    return this.BoolValue;

                case CallbackArgumentType.String:
                {
                    var buffer = new byte[this.Size];

                    Marshal.Copy(this.PointerValue, buffer, 0, buffer.Length);

                    return Encoding.ASCII.GetString(buffer).TrimEnd('\0');
                }

                default:
                    return null;
            }
        }
    }
}
