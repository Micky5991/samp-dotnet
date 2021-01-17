using System.Numerics;

namespace Micky5991.Samp.Net.Framework.Interfaces.Entities
{
    public interface IEntity
    {
        int Id { get; }

        Vector3 Position { get; set; }
    }
}
