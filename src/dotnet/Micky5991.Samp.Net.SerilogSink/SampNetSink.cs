using System;
using Micky5991.Samp.Net.Core.Interop;
using Serilog.Core;
using Serilog.Events;

namespace Micky5991.Samp.Net.SerilogSink
{
    public class SampNetSink : ILogEventSink
    {
        private readonly IFormatProvider? formatProvider;

        public SampNetSink(IFormatProvider? formatProvider)
        {
            this.formatProvider = formatProvider;
        }

        public void Emit(LogEvent logEvent)
        {
            var message = (string?) logEvent.RenderMessage(this.formatProvider);
            if (message == null)
            {
                return;
            }

            Native.LogMessage("TEST");
        }
    }
}
