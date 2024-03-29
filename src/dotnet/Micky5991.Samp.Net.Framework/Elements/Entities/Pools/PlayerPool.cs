using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Security.Claims;
using Dawn;
using Micky5991.Samp.Net.Framework.Constants;
using Micky5991.Samp.Net.Framework.Interfaces.Entities;
using Micky5991.Samp.Net.Framework.Interfaces.Entities.Factories;
using Micky5991.Samp.Net.Framework.Interfaces.Entities.Pools;

namespace Micky5991.Samp.Net.Framework.Elements.Entities.Pools
{
    /// <summary>
    /// Implements a player handler.
    /// </summary>
    public class PlayerPool : EntityPool<IPlayer>, IPlayerPool
    {
        private readonly IPlayerFactory playerFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="PlayerPool"/> class.
        /// </summary>
        /// <param name="playerFactory">Factory that creates <see cref="IPlayer"/> instances on demand.</param>
        public PlayerPool(IPlayerFactory playerFactory)
        {
            this.playerFactory = playerFactory;
        }

        /// <inheritdoc />
        public IPlayer CreateAndAddPlayer(int playerid)
        {
            Guard.Argument(playerid, nameof(playerid)).NotNegative();

            var player = this.playerFactory.CreatePlayer(playerid, this.RemoveEntity);

            this.SetupPlayerIdentity(player);

            this.AddEntity(player);

            return player;
        }

        /// <inheritdoc />
        public IPlayer? RemovePlayer(int playerid)
        {
            Guard.Argument(playerid, nameof(playerid)).NotNegative();

            IPlayer? result = null;

            this.UpdateEntities(c => c.TryGetValue(playerid, out result) ? c.Remove(playerid) : c);

            return result;
        }

        /// <inheritdoc />
        public ICollection<IPlayer> GetPlayersNearPoint(Vector3 position, float distance)
        {
            return this.Entities
                       .Where(entry => entry.Value.Valid() &&
                                       (position - entry.Value.Position).Length() < distance)
                       .Select(x => x.Value)
                       .ToList();
        }

        private void SetupPlayerIdentity(IPlayer player)
        {
            var principal = player.Principal;

            var sampIdentity = new ClaimsIdentity(
                                                  new[]
                                                  {
                                                      new Claim(SampClaimTypes.Name, player.Name),
                                                      new Claim(SampClaimTypes.PlayerId, player.Id.ToString(), ClaimValueTypes.Integer32),
                                                      new Claim(SampClaimTypes.IpAddress, player.Ip.ToString()),
                                                  });

            principal.AddIdentity(sampIdentity);
        }
    }
}
