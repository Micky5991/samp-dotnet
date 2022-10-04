using System;
using Serilog;
using Serilog.Configuration;

namespace Micky5991.Samp.Net.SerilogSink
{
    public static class SampSinkExtension
    {
        public static LoggerConfiguration SampLogFile(this LoggerSinkConfiguration loggerSinkConfiguration, IFormatProvider? formatProvider = null)
        {
            return loggerSinkConfiguration.Sink(new SampNetSink(formatProvider));
        }
    }
}
