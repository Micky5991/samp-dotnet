using System.Linq;
using AutoMapper;
using Micky5991.Samp.Net.Commands.Extensions;
using Micky5991.Samp.Net.Framework.Interfaces.Entities;
using Micky5991.Samp.Net.Framework.Interfaces.Pools;

namespace Micky5991.Samp.Net.Commands.Mapping.TypeConverters
{
    /// <summary>
    /// Converts string to <see cref="IPlayer"/> based on context.
    /// </summary>
    public class PlayerTypeConverter : ITypeConverter<string, IPlayer>
    {
        private readonly IPlayerPool playerPool;

        /// <summary>
        /// Initializes a new instance of the <see cref="PlayerTypeConverter"/> class.
        /// </summary>
        /// <param name="playerPool">Needed player pool for checks.</param>
        public PlayerTypeConverter(IPlayerPool playerPool)
        {
            this.playerPool = playerPool;
        }

        /// <inheritdoc/>
        public IPlayer Convert(string source, IPlayer destination, ResolutionContext context)
        {
            if (context.TryGetCommandExecutor(out var player))
            {
                if (source == "#")
                {
                    return player!;
                }
            }

            return this.playerPool.Entities.FirstOrDefault(x => x.Value.Name == source).Value!;
        }
    }
}
