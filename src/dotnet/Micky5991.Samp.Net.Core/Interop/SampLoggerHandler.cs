using System.Runtime.InteropServices;
using Micky5991.Samp.Net.Core.Interfaces.Logging;
using Microsoft.Extensions.Logging;

namespace Micky5991.Samp.Net.Core.Interop
{
    public class SampLoggerHandler : ISampLoggerHandler
    {
        private readonly ILogger<SampServer> logger;

        private bool attached;
        private GCHandle callbackHandle;

        public SampLoggerHandler(ILogger<SampServer> logger)
        {
            this.logger = logger;
        }

        public void Attach()
        {
            if (this.attached)
            {
                this.callbackHandle.Free();
            }

            Native.LoggerDelegate callback = this.Log;

            this.callbackHandle = GCHandle.Alloc(callback);
            Native.AttachLoggerHandler(callback);

            this.attached = true;
        }

        protected virtual void Log(string message)
        {
            this.logger.LogInformation(message);
        }

    }
}
