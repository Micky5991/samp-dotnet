using Micky5991.Samp.Net.Framework.Interfaces.Entities.Pools;

namespace Micky5991.Samp.Net.Framework.Interfaces.Entities.Factories
{
    /// <summary>
    /// Factory that will be used when a player instance should be created.
    /// </summary>
    public interface IPlayerFactory
    {
        /// <summary>
        /// Create player instance for specific player id.
        /// </summary>
        /// <param name="playerid">Player id that will be used to identity this entity.</param>
        /// <param name="entityRemovalDelegate">Callback that should be called when the entity has been disposed.</param>
        /// <returns>Created <see cref="IPlayer"/> instance.</returns>
        IPlayer CreatePlayer(int playerid, IPlayerPool.RemoveEntityDelegate entityRemovalDelegate);
    }
}
