using Micky5991.Samp.Net.Framework.Interfaces.Entities;

namespace Micky5991.Samp.Net.Framework.Interfaces.Pools
{
    /// <summary>
    /// Container that holds <see cref="IPlayer"/> instances.
    /// </summary>
    public interface IPlayerObjectPool : IEntityPool<IPlayerObject>
    {
    }
}
