using System;
using Micky5991.Samp.Net.Framework.Interfaces;
using Micky5991.Samp.Net.Framework.Utilities.Gamemodes;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Micky5991.Samp.Net.Example
{
    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                var serviceCollection = new ServiceCollection()
                    .AddLogging(builder => builder.AddConsole());

                new GamemodeBuilder()
                    .AddServices(serviceCollection);

                var serviceProvider = serviceCollection.BuildServiceProvider();

                var starter = serviceProvider.GetRequiredService<IGamemodeStarter>();

                starter.Start();
            }
            catch (Exception e)
            {
                Console.WriteLine($"ERROR: {e.Message}");
                Console.WriteLine(e.StackTrace);
            }
        }
    }
}
