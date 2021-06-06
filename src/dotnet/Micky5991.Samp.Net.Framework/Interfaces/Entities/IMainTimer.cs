using System;
using System.Timers;

namespace Micky5991.Samp.Net.Framework.Interfaces.Entities
{
    public interface IMainTimer : IDisposable
    {
        public event ElapsedEventHandler Elapsed;

        public event EventHandler Disposed;

        public void Start();
    }
}