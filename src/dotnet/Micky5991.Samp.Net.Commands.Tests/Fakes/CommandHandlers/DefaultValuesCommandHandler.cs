using Micky5991.Samp.Net.Commands.Attributes;
using Micky5991.Samp.Net.Commands.Interfaces;
using Micky5991.Samp.Net.Framework.Interfaces.Entities;

namespace Micky5991.Samp.Net.Commands.Tests.Fakes.CommandHandlers
{
    public class DefaultValuesCommandHandler : ICommandHandler
    {
        [Command("test", "grouped")]
        public void Command(IPlayer player, string test, int provided = 123)
        {
            // Empty
        }
    }
}
