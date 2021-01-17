﻿using System;
using System.Diagnostics;
using Micky5991.Samp.Net.Framework.Interfaces;
using Micky5991.Samp.Net.Framework.Utilities.Gamemodes;
using Micky5991.Samp.Net.NLogTarget;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;

namespace Micky5991.Samp.Net.Example
{
    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                SampLogTarget.Register();

                var serviceCollection = new ServiceCollection()
                    .AddLogging(builder =>
                    {
                        builder.SetMinimumLevel(LogLevel.Trace);
                        builder.AddNLog();
                    })
                    .AddSingleton<ChatListener>();

                new GamemodeBuilder()
                    .AddServices(serviceCollection);

                var serviceProvider = serviceCollection.BuildServiceProvider();

                var starter = serviceProvider.GetRequiredService<IGamemodeStarter>();

                starter
                    .StartLogRedirection()
                    .Start();

                var listener = serviceProvider.GetRequiredService<ChatListener>();
                listener.Attach();
            }
            catch (Exception e)
            {
                Console.WriteLine($"ERROR: {e.Message}");
                Console.WriteLine(e.StackTrace);
            }
        }
    }
}
