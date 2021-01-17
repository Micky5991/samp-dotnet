using Micky5991.Samp.Net.Framework.Interfaces.Entities;

namespace Micky5991.Samp.Net.Framework.Interfaces.Pools
{
    public interface IPlayerPool : IEntityPool<IPlayer>
    {
        IPlayer AddPlayer(int playerid);

        IPlayer? RemovePlayer(int playerid);
    }
}
