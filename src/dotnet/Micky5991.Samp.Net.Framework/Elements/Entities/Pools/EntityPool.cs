using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading;
using Dawn;
using Micky5991.Samp.Net.Framework.Interfaces.Entities;
using Micky5991.Samp.Net.Framework.Interfaces.Pools;

namespace Micky5991.Samp.Net.Framework.Elements.Entities.Pools
{
    /// <inheritdoc />
    public abstract class EntityPool<T> : IEntityPool<T>
        where T : IEntity
    {
        private readonly object enityModificationLock;

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityPool{T}"/> class.
        /// </summary>
        public EntityPool()
        {
            this.enityModificationLock = new ReaderWriterLock();

            this.Entities = new Dictionary<int, T>()
                .ToImmutableDictionary();
        }

        /// <inheritdoc />
        public IImmutableDictionary<int, T> Entities { get; private set; }

        /// <inheritdoc />
        public T FindOrCreateEntity(int id)
        {
            Guard.Argument(id, nameof(id)).NotNegative();

            throw new System.NotImplementedException();
        }

        /// <inheritdoc />
        public T? FindOrDefaultEntity(int id)
        {
            if (this.Entities.TryGetValue(id, out var entity) == false)
            {
                return default;
            }

            return entity;
        }

        /// <summary>
        /// Modifies the entity collection in this pool safely.
        /// </summary>
        /// <param name="modificator">Callback which will be called so you can modify it in a safe way.</param>
        /// <exception cref="ArgumentNullException"><paramref name="modificator"/> was null.</exception>
        protected void UpdateEntities(Func<IImmutableDictionary<int, T>, IImmutableDictionary<int, T>> modificator)
        {
            Guard.Argument(modificator, nameof(modificator)).NotNull();

            lock (this.enityModificationLock)
            {
                this.Entities = modificator(this.Entities);
            }
        }

        /// <summary>
        /// Adds a specific entity to the current pool.
        /// </summary>
        /// <param name="entity">Entity to add.</param>
        /// <exception cref="ArgumentException">The given entity was already added to the pool.</exception>
        protected void AddEntity(T entity)
        {
            this.UpdateEntities(collection => collection.Add(entity.Id, entity));
        }

        /// <summary>
        /// Removes a specific entity from this pool.
        /// </summary>
        /// <param name="entity">Entity to remove.</param>
        protected void RemoveEntity(T entity)
        {
            this.UpdateEntities(collection => collection.Remove(entity.Id));
        }
    }
}
