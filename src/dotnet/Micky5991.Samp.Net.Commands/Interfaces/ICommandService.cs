using System.Collections.Immutable;
using Micky5991.EventAggregator.Interfaces;

namespace Micky5991.Samp.Net.Commands.Interfaces
{
    /// <summary>
    /// Handler for all registered <see cref="ICommandHandler"/> instances.
    /// </summary>
    public interface ICommandService
    {
        /// <summary>
        /// Gets all available commands in this service instance.
        /// </summary>
        IImmutableDictionary<string, IImmutableDictionary<string, ICommand>> Commands { get; }

        /// <summary>
        /// Starts all <see cref="ICommandHandler"/> instances and attaches the command listener to the <see cref="IEventAggregator"/>.
        /// </summary>
        void Start();
    }
}
