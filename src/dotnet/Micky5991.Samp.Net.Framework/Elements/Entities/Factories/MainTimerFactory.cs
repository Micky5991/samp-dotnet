using System;
using Dawn;
using Micky5991.Samp.Net.Core.Threading;
using Micky5991.Samp.Net.Framework.Interfaces.Entities;
using Micky5991.Samp.Net.Framework.Interfaces.Entities.Factories;

namespace Micky5991.Samp.Net.Framework.Elements.Entities.Factories
{
    /// <inheritdoc />
    public class MainTimerFactory : IMainTimerFactory
    {
        private readonly SampSynchronizationContext sampSynchronizationContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainTimerFactory"/> class.
        /// </summary>
        /// <param name="sampSynchronizationContext">Main Thread of SA:MP.</param>
        public MainTimerFactory(SampSynchronizationContext sampSynchronizationContext)
        {
            this.sampSynchronizationContext = sampSynchronizationContext;
        }

        /// <inheritdoc />
        public IMainTimer CreateTimer(TimeSpan interval, bool repeating)
        {
            Guard.Argument(interval).Min(TimeSpan.FromMilliseconds(1));

            return new MainTimer(interval, repeating, this.sampSynchronizationContext);
        }
    }
}
