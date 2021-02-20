using System;

namespace Micky5991.Samp.Net.Framework.Interfaces
{
    public interface IGamemode
    {
        IServiceProvider ServiceProvider { get; }

        void Start();
    }
}
