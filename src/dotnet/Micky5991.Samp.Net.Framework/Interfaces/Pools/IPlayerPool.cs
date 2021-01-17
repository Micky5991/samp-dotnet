using Micky5991.Samp.Net.Framework.Interfaces.Entities;

namespace Micky5991.Samp.Net.Framework.Interfaces.Pools
{
    /// <summary>
    /// Container that holds <see cref="IPlayer"/> instances.
    /// </summary>
    public interface IPlayerPool : IEntityPool<IPlayer>
    {
        /// <summary>
        /// Creates and adds a player with this specific id to the pool.
        /// </summary>
        /// <param name="playerid">Id of the player to add.</param>
        /// <returns>The created player.</returns>
        IPlayer CreateAndAddPlayer(int playerid);

        /// <summary>
        /// Removes and returns the player with this specific id.
        /// </summary>
        /// <param name="playerid">Id of the player to search and remove.</param>
        /// <returns>Instance of <see cref="IPlayer"/> if player is in the pool, <value>null</value> otherwise.</returns>
        IPlayer? RemovePlayer(int playerid);
    }
}
