using System.Collections.Immutable;
using Micky5991.Samp.Net.Commands.Interfaces;

namespace Micky5991.Samp.Net.Commands.Tests.Fakes
{
    public class FakeCommandService : ICommandService
    {
        public IImmutableDictionary<string, IImmutableDictionary<string, ICommand>> Commands { get; }

        public IImmutableDictionary<string, IImmutableDictionary<string, ICommand>> NonAliasCommands { get; }

        public void Start()
        {
            throw new System.NotImplementedException();
        }
    }
}
