using System;
using System.Timers;

namespace Micky5991.Samp.Net.Framework.Interfaces.Entities
{
    /// <summary>
    /// Timer which elapsed event handler will be called in the main thread.
    /// </summary>
    public interface IMainTimer : IDisposable
    {
        /// <summary>
        /// Event when an interval of the timer has elapsed.
        /// </summary>
        public event ElapsedEventHandler Elapsed;

        /// <summary>
        /// Event that triggers when the timer has been disposed.
        /// </summary>
        public event EventHandler Disposed;

        /// <summary>
        /// Starts the timer and begins to trigger <see cref="Elapsed"/>.
        /// </summary>
        /// <exception cref="ObjectDisposedException">Object has been disposed.</exception>
        public void Start();

        /// <summary>
        /// Stops the timer and stops triggering <see cref="Elapsed"/>.
        /// </summary>
        /// <exception cref="ObjectDisposedException">Object has been disposed.</exception>
        public void Stop();
    }
}
