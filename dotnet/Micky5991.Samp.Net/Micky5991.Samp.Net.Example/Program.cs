using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using Micky5991.EventAggregator.Interfaces;
using Micky5991.EventAggregator.Services;
using Micky5991.Samp.Net.Core;
using Micky5991.Samp.Net.Core.Interfaces.Events;
using Micky5991.Samp.Net.Core.Interop;
using Micky5991.Samp.Net.Core.Interop.Events;
using Micky5991.Samp.Net.Core.Natives.Actor;
using Micky5991.Samp.Net.Core.Natives.Objects;
using Micky5991.Samp.Net.Core.Natives.Players;
using Micky5991.Samp.Net.Core.Natives.Samp;
using Micky5991.Samp.Net.Core.Natives.Vehicles;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Micky5991.Samp.Net.Example
{
    public class Program
    {
        private static GCHandle handle;

        private static SampSynchronizationContext context;

        public static void Main(string[] args)
        {
            context = SampSynchronizationContext.Setup();

            Native.PluginTickDelegate callback = Tick;

            GCHandle.Alloc(callback);

            Native.AttachTickHandler(callback);

            try
            {

                var serviceCollection = new ServiceCollection()
                                        .AddLogging(builder => builder.AddConsole())

                                        .AddSingleton<IEventAggregator, EventAggregatorService>()

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
                eventAggregator.SetMainThreadSynchronizationContext(SynchronizationContext.Current);

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

                eventAggregator.Subscribe<NativeGameModeInitEvent>(async x =>
                {
                    var context = SynchronizationContext.Current;

                    Console.WriteLine($"INIT Context: {(context == null ? "NULL" : "NOT NULL")}");

                    Console.WriteLine($"A1 - {Thread.CurrentThread.ManagedThreadId}");
                    await Task.Run(() => Console.WriteLine($"B - {Thread.CurrentThread.ManagedThreadId}"));
                    Console.WriteLine($"A2 - {Thread.CurrentThread.ManagedThreadId}");
                });

                eventAggregator.Subscribe<NativePlayerConnectEvent>(x =>
                {
                    playersNatives.GetPlayerName(x.Playerid, out var name, 25);

                    Console.WriteLine($"Player {name} joined.");
                });

                eventAggregator.Subscribe<NativePlayerEnterVehicleEvent>(x =>
                {
                    playersNatives.GetPlayerName(x.Playerid, out var name, 25);

                    Console.WriteLine($"Player {x.Playerid} entered vehicle as {(x.Ispassenger ? "Passenger" : "Driver")}");
                });

                eventAggregator.Subscribe<NativeIncomingConnectionEvent>(x =>
                {
                    x.Cancelled = true;

                    Console.WriteLine($"Incoming: {x.IpAddress}");
                });

                eventAggregator.Subscribe<NativePlayerTextEvent>(x =>
                {
                    x.Cancelled = false;

                    sampNatives.SendClientMessage(x.Playerid, -1, $"Du hast folgendes geschrieben: {x.Text}");
                    Console.WriteLine($"CHAT {x.Playerid}: {x.Text}");
                });

                // Console.WriteLine($"A1 - {Thread.CurrentThread.ManagedThreadId}");
                //
                // await RunSomething();
                //
                // Console.WriteLine($"A2 - {Thread.CurrentThread.ManagedThreadId}");
            }
            catch (Exception e)
            {
                Console.WriteLine($"ERROR: {e.Message}");
                Console.WriteLine(e.StackTrace);
            }
        }

        public static async Task RunSomething()
        {
            Console.WriteLine($"B1 - {Thread.CurrentThread.ManagedThreadId}");

            await Task.Delay(1000);

            Console.WriteLine($"B2 - {Thread.CurrentThread.ManagedThreadId}");
        }

        public static void Tick()
        {
            context.Run();
        }
    }
}
