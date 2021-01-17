using System;
using System.Numerics;

namespace Micky5991.Samp.Net.Framework.Interfaces.Entities
{
    /// <summary>
    /// Represents an entity that can be represented in the world with a specific position.
    /// </summary>
    public interface IWorldEntity : IEntity
    {
        /// <summary>
        /// Gets or sets the current position in the world of this entity.
        /// </summary>
        /// <exception cref="ObjectDisposedException"><see cref="IWorldEntity"/> was disposed.</exception>
        public Vector3 Position { get; set; }

        /// <summary>
        /// Gets or sets the current rotation of this entity.
        /// </summary>
        /// <exception cref="ObjectDisposedException"><see cref="IWorldEntity"/> was disposed.</exception>
        float Rotation { get; set; }
    }
}
