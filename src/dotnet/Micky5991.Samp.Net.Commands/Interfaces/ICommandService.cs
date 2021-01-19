using Micky5991.EventAggregator.Interfaces;

namespace Micky5991.Samp.Net.Commands.Interfaces
{
    /// <summary>
    /// Handler for all registered <see cref="ICommandHandler"/> instances.
    /// </summary>
    public interface ICommandService
    {
        /// <summary>
        /// Starts all <see cref="ICommandHandler"/> instances and attaches the command listener to the <see cref="IEventAggregator"/>.
        /// </summary>
        void Start();
    }
}
