using System;
using Micky5991.Samp.Net.Framework.Interfaces.Entities;

namespace Micky5991.Samp.Net.Framework.Entities
{
    /// <inheritdoc />
    public abstract class Entity : IEntity
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Entity"/> class.
        /// </summary>
        /// <param name="id">Current id of this entity.</param>
        /// <param name="entityRemoval">Delegate that should be called when this entity was disposed.</param>
        protected Entity(int id)
        {
            this.Id = id;
        }

        /// <inheritdoc />
        public int Id { get; }

        /// <inheritdoc />
        public bool Disposed { get; private set; }

        /// <inheritdoc />
        public void Dispose()
        {
            if (this.Disposed)
            {
                throw new ObjectDisposedException(nameof(Entity));
            }

            this.DisposeEntity();

            this.Disposed = true;

            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Executes an actual dispose of this entity.
        /// </summary>
        protected abstract void DisposeEntity();
    }
}
