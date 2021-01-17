namespace Micky5991.Samp.Net.Framework.Interfaces.Entities
{
    /// <summary>
    /// Represents an entity that has a master player which it is attached to.
    /// </summary>
    public interface IPlayerBoundEnitity : IEntity
    {
        /// <summary>
        /// Gets the player that owns this entity.
        /// </summary>
        IPlayer Player { get; }
    }
}
