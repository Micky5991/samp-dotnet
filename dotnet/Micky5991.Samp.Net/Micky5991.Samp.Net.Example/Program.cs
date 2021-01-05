using System;
using System.Runtime.InteropServices;
using Micky5991.EventAggregator.Extensions;
using Micky5991.EventAggregator.Interfaces;
using Micky5991.Samp.Net.Core.Interfaces.Events;
using Micky5991.Samp.Net.Core.Interop;
using Micky5991.Samp.Net.Core.Interop.Events;
using Micky5991.Samp.Net.Core.Natives;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Micky5991.Samp.Net.Example
{
    public class Program
    {
        private static GCHandle handle;

        public static void Main(string[] args)
        {
            try
            {

                var serviceCollection = new ServiceCollection()
                                        .AddEventAggregator()
                                        .AddLogging(builder => builder.AddConsole())

                                        .AddTransient<NativeTypeConverter>()
                                        .AddSingleton<INativeEventRegistry, NativeEventRegistry>()

                                        .AddTransient<INativeEventCollectionFactory, SampEventCollectionFactory>()
                                        .AddTransient<INativeEventCollectionFactory, VehiclesEventCollectionFactory>()
                                        .AddTransient<INativeEventCollectionFactory, PlayersEventCollectionFactory>()
                                        .AddTransient<INativeEventCollectionFactory, ActorEventCollectionFactory>()
                                        .AddTransient<INativeEventCollectionFactory, ObjectsEventCollectionFactory>()

                                        .AddTransient<ISampNatives, SampNatives>()
                                        .AddTransient<IVehiclesNatives, VehiclesNatives>()
                                        .AddTransient<IPlayersNatives, PlayersNatives>()
                                        .AddTransient<IActorNatives, ActorNatives>()
                                        .AddTransient<IObjectsNatives, ObjectsNatives>();

                var serviceProvider = serviceCollection.BuildServiceProvider();

                var eventRegistry = serviceProvider.GetRequiredService<INativeEventRegistry>();
                var eventAggregator = serviceProvider.GetRequiredService<IEventAggregator>();

                eventRegistry.AttachEventInvoker();
                foreach (var factory in serviceProvider.GetServices<INativeEventCollectionFactory>())
                {
                    eventRegistry.RegisterEvents(factory.Build());
                }

                var sampNatives = serviceProvider.GetRequiredService<ISampNatives>();
                var vehiclesNatives = serviceProvider.GetRequiredService<IVehiclesNatives>();
                var playersNatives = serviceProvider.GetRequiredService<IPlayersNatives>();

                sampNatives.SetGameModeText("C# Test");

                var vehicle = vehiclesNatives.CreateVehicle(541, 0, 0, 0, 268.8108f, 0, 0, 0, true);

                vehiclesNatives.SetVehicleNumberPlate(vehicle, "LUL");

                eventAggregator.SubscribeSync<NativePlayerConnectEvent>(x =>
                {
                    playersNatives.GetPlayerName(x.Playerid, out var name, 25);
                    if (name == null)
                    {
                        Console.WriteLine("NAME NULL");

                        return;
                    }

                    Console.WriteLine($"Player {name} joined.");
                });

                eventAggregator.SubscribeSync<NativePlayerEnterVehicleEvent>(x =>
                {
                    playersNatives.GetPlayerName(x.Playerid, out var name, 25);
                    if (name == null)
                    {
                        Console.WriteLine("Null");
                    }

                    Console.WriteLine($"Player {x.Playerid} entered vehicle as {(x.Ispassenger ? "Passenger" : "Driver")}");
                });
            }
            catch (Exception e)
            {
                Console.WriteLine($"ERROR: {e.Message}");
                Console.WriteLine(e.StackTrace);
            }
        }
    }
}
