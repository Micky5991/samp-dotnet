namespace Micky5991.Samp.Net.Framework.Interfaces
{
    /// <summary>
    /// Listens to certain entity events and executes actions on them.
    /// </summary>
    public interface IEntityListener
    {
        /// <summary>
        /// Starts the listener and setups actions.
        /// </summary>
        void Attach();
    }
}
