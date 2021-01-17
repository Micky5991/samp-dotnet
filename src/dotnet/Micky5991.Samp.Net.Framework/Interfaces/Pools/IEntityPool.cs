using System.Collections.Generic;
using Micky5991.Samp.Net.Framework.Interfaces.Entities;

namespace Micky5991.Samp.Net.Framework.Interfaces.Pools
{
    public interface IEntityPool<T> : IReadOnlyList<T>
        where T : IEntity
    {
        new T? this[int index] { get; }
    }
}
