using System.Runtime.CompilerServices;

namespace Micky5991.Samp.Net.Core.Interfaces.Interop
{
    public interface ISampThreadEnforcer
    {
        int? MainThread { get; }

        void SetMainThread();

        void EnforceMainThread([CallerMemberName] string callerMemberName = "");
    }
}
