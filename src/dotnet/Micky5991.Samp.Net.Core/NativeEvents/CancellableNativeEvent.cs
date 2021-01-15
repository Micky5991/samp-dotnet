using Micky5991.EventAggregator.Interfaces;

namespace Micky5991.Samp.Net.Core.NativeEvents
{
    public class CancellableNativeEvent : NativeEvent, ICancellableEvent
    {
        public bool Cancelled { get; set; }
    }
}
