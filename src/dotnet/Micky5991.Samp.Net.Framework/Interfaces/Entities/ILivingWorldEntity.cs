using System;

namespace Micky5991.Samp.Net.Framework.Interfaces.Entities
{
    /// <summary>
    /// Defines properties which are used in world entities that have health.
    /// </summary>
    public interface ILivingWorldEntity : IWorldEntity
    {
        /// <summary>
        /// Gets or sets the current health of this entity.
        /// </summary>
        /// <exception cref="ObjectDisposedException"><see cref="ILivingWorldEntity"/> is disposed.</exception>
        float Health { get; set; }
    }
}
