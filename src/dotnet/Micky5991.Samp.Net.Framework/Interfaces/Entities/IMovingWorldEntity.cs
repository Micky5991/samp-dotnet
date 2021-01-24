using System;
using System.Numerics;

namespace Micky5991.Samp.Net.Framework.Interfaces.Entities
{
    /// <summary>
    /// Defines properties which are used in world entities that can move.
    /// </summary>
    public interface IMovingWorldEntity : IWorldEntity
    {
        /// <summary>
        /// Gets or sets the velocity of this entity.
        /// </summary>
        /// <exception cref="ObjectDisposedException"><see name="IMovingWorldEntity"/> was disposed.</exception>
        Vector3 Velocity { get; set; }

        /// <summary>
        /// Gets or sets this entity into the specific virtual world.
        /// </summary>
        /// <exception cref="ObjectDisposedException"><see name="IMovingWorldEntity"/> was disposed.</exception>
        int VirtualWorld { get; set; }
    }
}
