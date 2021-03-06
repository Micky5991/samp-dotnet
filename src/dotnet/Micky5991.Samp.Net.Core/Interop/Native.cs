using System;
using System.Runtime.InteropServices;
using Micky5991.Samp.Net.Core.Interop.Data;
using Micky5991.Samp.Net.Core.Interop.Events;

namespace Micky5991.Samp.Net.Core.Interop
{
    public static class Native
    {
        private const CallingConvention CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall;

        private const string Plugin = "samp-dotnet";

        public delegate EventInvokeResult EventInvokerDelegate([MarshalAs(UnmanagedType.LPStr)]string eventName, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 2)] CallbackArgument[]? argments, int argumentAmount);
        public delegate void LoggerDelegate([MarshalAs(UnmanagedType.LPStr)]string message);

        public delegate void PluginTickDelegate();

        [DllImport(Plugin, CallingConvention = CallingConvention)]
        public static extern int InvokeNative([MarshalAs(UnmanagedType.LPStr)] string nativeName,
                                              [MarshalAs(UnmanagedType.LPStr)] string format,
                                              IntPtr arguments);

        [DllImport(Plugin, CallingConvention = CallingConvention)]
        public static extern int RegisterEvent([MarshalAs(UnmanagedType.LPStr)] string eventName,
                                               [MarshalAs(UnmanagedType.LPStr)] string format);

        [DllImport(Plugin, CallingConvention = CallingConvention)]
        public static extern int AttachEventHandler([MarshalAs(UnmanagedType.FunctionPtr)] EventInvokerDelegate callback);

        [DllImport(Plugin, CallingConvention = CallingConvention)]
        public static extern int AttachTickHandler([MarshalAs(UnmanagedType.FunctionPtr)] PluginTickDelegate callback);

        [DllImport(Plugin, CallingConvention = CallingConvention)]
        public static extern void AttachLoggerHandler([MarshalAs(UnmanagedType.FunctionPtr)] LoggerDelegate logger);

        [DllImport(Plugin, CallingConvention = CallingConvention)]
        public static extern void LogMessage([MarshalAs(UnmanagedType.LPStr)] string message);

    }
}
