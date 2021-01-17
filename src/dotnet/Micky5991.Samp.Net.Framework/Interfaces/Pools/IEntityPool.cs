using System.Collections.Immutable;
using Micky5991.Samp.Net.Framework.Interfaces.Entities;

namespace Micky5991.Samp.Net.Framework.Interfaces.Pools
{
    /// <summary>
    /// Pool that handles current available entities on this server.
    /// </summary>
    /// <typeparam name="T">Entity type that will be contained in this pool.</typeparam>
    public interface IEntityPool<T>
        where T : IEntity
    {
        /// <summary>
        /// Delegate that passes a callback on where to remove an entity from.
        /// </summary>
        /// <param name="entity">Entity to remove.</param>
        public delegate void RemoveEntityDelegate(T entity);

        /// <summary>
        /// Gets list of all known entites on this server. Important: If a vehicle was created outside the gamemode, like a
        /// filterscript, the pool could have skipped this entity.
        /// </summary>
        IImmutableDictionary<int, T> Entities { get; }

        /// <summary>
        /// Requests an entity, but if the entity is not available, the pool will try to register one.
        /// </summary>
        /// <param name="id">Id of the entity.</param>
        /// <returns>Found or created entity of type <typeparamref name="T"/>.</returns>
        T FindOrCreateEntity(int id);
    }
}
