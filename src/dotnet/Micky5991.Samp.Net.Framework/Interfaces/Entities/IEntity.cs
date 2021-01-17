using System;

namespace Micky5991.Samp.Net.Framework.Interfaces.Entities
{
    /// <summary>
    /// Represents any samp entity.
    /// </summary>
    public interface IEntity : IDisposable
    {
        /// <summary>
        /// Gets current non-negative id of this entity.
        /// </summary>
        /// <exception cref="ObjectDisposedException"><see cref="Disposed"/> is true.</exception>
        int Id { get; }

        /// <summary>
        /// Gets a value indicating whether the current entity has been disposed.
        /// </summary>
        bool Disposed { get; }

        /// <summary>
        /// Gets if the entity is still valid.
        /// </summary>
        /// <returns>true if the instance has not been disposed, false otherwise.</returns>
        bool Valid();
    }
}
