using Micky5991.Samp.Net.Framework.Utilities.Gamemodes;
using Micky5991.Samp.Net.NLogTarget;

namespace Micky5991.Samp.Net.Example
{
    public class Program
    {
        public static void Main(string[] args)
        {
            SampLogTarget.Register();

            new GamemodeBuilder()
                    .UseStartup<Startup>()
                    .Build(args)
                    .Start();
        }
    }
}
