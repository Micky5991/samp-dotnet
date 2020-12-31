using System;
using System.Runtime.InteropServices;

namespace Micky5991.Samp.Net.Core.Native
{
    public static class Native
    {
        private const CallingConvention _callingConvention = CallingConvention.StdCall;

        private const string _plugin = "samp-dotnet";

        [DllImport(_plugin, CallingConvention = _callingConvention)]
        public static extern int InvokeNative([MarshalAs(UnmanagedType.LPStr)] string nativeName,
                                               [MarshalAs(UnmanagedType.LPStr)] string format,
                                               IntPtr arguments);

    }
}
