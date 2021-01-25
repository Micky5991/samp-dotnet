using System.Collections.Generic;
using System.Numerics;

namespace Micky5991.Samp.Net.Framework.Interfaces.Entities.Pools
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

        /// <summary>
        /// Returns a list of players which are close to this position.
        /// </summary>
        /// <param name="position">Center position of this circle.</param>
        /// <param name="distance">Maximum radius to the player.</param>
        /// <returns><see cref="IPlayer"/> in this range.</returns>
        ICollection<IPlayer> GetPlayersNearPoint(Vector3 position, float distance);
    }
}
