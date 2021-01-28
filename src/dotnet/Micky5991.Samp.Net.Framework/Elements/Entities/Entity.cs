using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading;
using Dawn;
using Micky5991.Samp.Net.Framework.Interfaces.Entities;

namespace Micky5991.Samp.Net.Framework.Elements.Entities
{
    /// <inheritdoc />
    public abstract partial class Entity : IEntity
    {
        private readonly int id;

        private readonly CancellationTokenSource cancellationTokenSource;

        /// <summary>
        /// Initializes a new instance of the <see cref="Entity"/> class.
        /// </summary>
        /// <param name="id">Current id of this entity.</param>
        protected Entity(int id)
        {
            this.id = id;

            this.Data = new Dictionary<string, object?>().ToImmutableDictionary();

            this.cancellationTokenSource = new CancellationTokenSource();
        }

        /// <inheritdoc />
        public int Id
        {
            get
            {
                Guard.Disposal(this.Disposed);

                return this.id;
            }
        }

        /// <inheritdoc />
        public bool Disposed { get; private set; }

        /// <inheritdoc />
        public CancellationToken CancellationToken => this.cancellationTokenSource.Token;

        /// <inheritdoc />
        public bool Valid()
        {
            return this.Disposed == false;
        }

        /// <inheritdoc />
        public void Dispose()
        {
            Guard.Disposal(this.Disposed);

            this.cancellationTokenSource.Cancel();

            this.DisposeEntity();

            this.Disposed = true;

            GC.SuppressFinalize(this);
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return $"<{this.GetType()} ({this.Id})>";
        }

        /// <summary>
        /// Executes an actual dispose of this entity.
        /// </summary>
        protected abstract void DisposeEntity();
    }
}
