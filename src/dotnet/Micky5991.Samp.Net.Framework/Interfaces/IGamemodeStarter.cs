namespace Micky5991.Samp.Net.Framework.Interfaces
{
    /// <summary>
    /// Gamemode starter that takes the core services and starts them.
    /// </summary>
    public interface IGamemodeStarter
    {
        /// <summary>
        /// Starts core services and bootstraps with the native part of this plugin.
        /// </summary>
        /// <returns>Current <see cref="IGamemodeStarter"/> instance.</returns>
        IGamemodeStarter Start();
    }
}
