using System;
using System.Threading;
using System.Timers;
using Dawn;
using Micky5991.Samp.Net.Framework.Interfaces.Entities;
using Timer = System.Timers.Timer;

namespace Micky5991.Samp.Net.Framework.Elements.Entities
{
    /// <inheritdoc />
    public class MainTimer : IMainTimer
    {
        private readonly SynchronizationContext synchronizationContext;

        private readonly Timer timer;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainTimer"/> class.
        /// </summary>
        /// <param name="interval">Interval the timer should be repeated on.</param>
        /// <param name="repeating">Flag if the timer should repeatedly execute the <see cref="Elapsed"/> event.</param>
        /// <param name="synchronizationContext">Main thread to execute the timer in.</param>
        public MainTimer(TimeSpan interval, bool repeating, SynchronizationContext synchronizationContext)
        {
            Guard.Argument(interval).Min(TimeSpan.FromMilliseconds(1));
            Guard.Argument(synchronizationContext).NotNull();

            this.synchronizationContext = synchronizationContext;

            this.timer = new Timer(interval.TotalMilliseconds);
            this.timer.Elapsed += this.OnElapsed;
            this.timer.Disposed += this.OnDisposed;
            this.timer.AutoReset = repeating;
        }

        /// <inheritdoc />
        public event ElapsedEventHandler? Elapsed;

        /// <inheritdoc />
        public event EventHandler? Disposed;

        /// <inheritdoc />
        public void Start()
        {
            this.timer.Start();
        }

        /// <inheritdoc />
        public void Stop()
        {
            this.timer.Stop();
        }

        /// <inheritdoc />
        public void Dispose()
        {
            this.timer.Dispose();
        }

        private void OnDisposed(object sender, EventArgs args)
        {
            if (this.Disposed == null)
            {
                return;
            }

            this.synchronizationContext.Post(
                                             _ => this.Disposed.Invoke(sender, args),
                                             null);
        }

        private void OnElapsed(object sender, ElapsedEventArgs args)
        {
            if (this.Elapsed == null)
            {
                return;
            }

            this.synchronizationContext.Post(
                                             _ => this.Elapsed.Invoke(sender, args),
                                             null);
        }
    }
}
