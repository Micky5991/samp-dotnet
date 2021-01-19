using AutoMapper;
using Micky5991.Samp.Net.Commands.Mapping.TypeConverters;
using Micky5991.Samp.Net.Framework.Interfaces.Entities;

namespace Micky5991.Samp.Net.Commands.Mapping
{
    /// <summary>
    /// Sets default samp conversions needed for command parsing.
    /// </summary>
    public class SampCommandMappingProfile : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SampCommandMappingProfile"/> class.
        /// </summary>
        public SampCommandMappingProfile()
        {
            this.CreateMap<string, IPlayer>().ConvertUsing<PlayerTypeConverter>();
        }
    }
}
