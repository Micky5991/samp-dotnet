using System;
using System.Runtime.InteropServices;
using Micky5991.Samp.Net.Core.Interfaces.Events;

namespace Micky5991.Samp.Net.Core.Interop
{
    public static class Native
    {
        private const CallingConvention _callingConvention = CallingConvention.StdCall;

        private const string _plugin = "samp-dotnet";

        [DllImport(_plugin, CallingConvention = _callingConvention)]
        public static extern int InvokeNative([MarshalAs(UnmanagedType.LPStr)] string nativeName,
                                              [MarshalAs(UnmanagedType.LPStr)] string format,
                                              IntPtr arguments);

        [DllImport(_plugin, CallingConvention = _callingConvention)]
        public static extern int RegisterEvent([MarshalAs(UnmanagedType.LPStr)] string eventName,
                                               [MarshalAs(UnmanagedType.LPStr)] string format);

        [DllImport(_plugin, CallingConvention = _callingConvention)]
        public static extern int AttachEventHandler([MarshalAs(UnmanagedType.FunctionPtr)] INativeEventRegistry.EventInvokerDelegate callback);

    }
}
