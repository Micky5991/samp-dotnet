namespace Micky5991.Samp.Net.Commands.Interfaces
{
    /// <summary>
    /// Describes a listener which will be started before starting the <see cref="ICommandService"/>.
    /// </summary>
    public interface ICommandListener
    {
        /// <summary>
        /// Starts listening to specific events.
        /// </summary>
        void Listen();
    }
}
