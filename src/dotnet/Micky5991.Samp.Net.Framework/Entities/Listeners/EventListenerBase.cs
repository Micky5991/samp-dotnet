using Micky5991.EventAggregator.Interfaces;
using Micky5991.Samp.Net.Framework.Interfaces;

namespace Micky5991.Samp.Net.Framework.Entities.Listeners
{
    /// <summary>
    /// Base that provides general methods for easy handling.
    /// </summary>
    public abstract class EventListenerBase : IEntityListener
    {
        private readonly IEventAggregator eventAggregator;

        /// <summary>
        /// Initializes a new instance of the <see cref="EventListenerBase"/> class.
        /// </summary>
        /// <param name="eventAggregator">Evnt aggregator used for event triggering.</param>
        protected EventListenerBase(IEventAggregator eventAggregator)
        {
            this.eventAggregator = eventAggregator;
        }

        /// <inheritdoc />
        public abstract void Attach();

        /// <summary>
        /// Copies the cancellation status from <paramref name="sourceEvent"/> to <paramref name="targetEvent"/> and vice-versa.
        /// </summary>
        /// <param name="sourceEvent">Already triggered event.</param>
        /// <param name="targetEvent">Target event where the resulting cancellation should be copied to and from after publishing.</param>
        protected void WrapCancellableEvent(ICancellableEvent sourceEvent, ICancellableEvent targetEvent)
        {
            targetEvent.Cancelled = sourceEvent.Cancelled;

            this.eventAggregator.Publish(targetEvent);

            sourceEvent.Cancelled = targetEvent.Cancelled;
        }
    }
}
