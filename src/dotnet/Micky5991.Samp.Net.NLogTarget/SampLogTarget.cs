using Micky5991.Samp.Net.Core.Interop;
using NLog;
using NLog.Targets;

namespace Micky5991.Samp.Net.NLogTarget
{
    [Target("Samp")]
    public class SampLogTarget : TargetWithLayout
    {
        public static void Register()
        {
            Register<SampLogTarget>("Samp");
        }

        protected override void Write(LogEventInfo logEvent)
        {
            var message = this.RenderLogEvent(this.Layout, logEvent);

            if (message == null)
            {
                return;
            }

            Native.LogMessage(message);
        }
    }
}
