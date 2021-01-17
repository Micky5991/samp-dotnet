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
        public Vector3 Position { get; set; }
    }
}
