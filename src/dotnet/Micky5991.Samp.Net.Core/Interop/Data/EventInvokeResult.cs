using System.Runtime.InteropServices;

namespace Micky5991.Samp.Net.Core.Interop.Data
{
    public struct EventInvokeResult
    {
        [MarshalAs(UnmanagedType.U1)]
        public bool AllowExecute;

        [MarshalAs(UnmanagedType.U1)]
        public bool ReturnValue;

        public EventInvokeResult(bool allowExecute, bool returnValue)
        {
            this.AllowExecute = allowExecute;
            this.ReturnValue = returnValue;
        }
    }
}
