using System;
using Micky5991.Samp.Net.Commands;
using Micky5991.Samp.Net.Framework.Utilities.Gamemodes;
using Micky5991.Samp.Net.Framework.Utilities.Startup;
using Micky5991.Samp.Net.NLogTarget;

namespace Micky5991.Samp.Net.Example
{
    public class Program
    {
        public static void Main(string[] args)
        {
            SampLogTarget.Register();

            Console.WriteLine("Starting Example");

            new StartupDirector(new ExampleServerBuilder())
                .AddGamemodeBuilder(new CoreGamemodeBuilder())
                .AddGamemodeBuilder(
                                    new CommandExtensionBuilder()
                                        .AddDefaultMappingProfiles()
                                        .AddDefaultCommands()
                                   )
                .Build()
                .Start();
        }
    }
}
