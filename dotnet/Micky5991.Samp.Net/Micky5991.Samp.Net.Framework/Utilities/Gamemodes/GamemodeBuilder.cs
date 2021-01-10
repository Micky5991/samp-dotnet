using Micky5991.EventAggregator.Interfaces;
using Micky5991.EventAggregator.Services;
using Micky5991.Samp.Net.Core.Interfaces.Events;
using Micky5991.Samp.Net.Core.Interop;
using Micky5991.Samp.Net.Core.Interop.Events;
using Micky5991.Samp.Net.Core.Natives.Actor;
using Micky5991.Samp.Net.Core.Natives.Objects;
using Micky5991.Samp.Net.Core.Natives.Players;
using Micky5991.Samp.Net.Core.Natives.Samp;
using Micky5991.Samp.Net.Core.Natives.Vehicles;
using Micky5991.Samp.Net.Core.Threading;
using Micky5991.Samp.Net.Framework.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Micky5991.Samp.Net.Framework.Utilities.Gamemodes
{
    public class GamemodeBuilder
    {
        public virtual void AddServices(IServiceCollection serviceCollection)
        {
            this.AddGamemodeStarter(serviceCollection);
            this.AddEventAggregator(serviceCollection);
            this.AddSynchronizationServices(serviceCollection);
            this.AddNativeEventHandling(serviceCollection);
            this.AddNativeEvents(serviceCollection);
            this.AddNatives(serviceCollection);
        }

        protected virtual void AddGamemodeStarter(IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<IGamemodeStarter, GamemodeStarter>();
        }

        protected virtual void AddEventAggregator(IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<IEventAggregator, EventAggregatorService>();
        }

        protected virtual void AddSynchronizationServices(IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<SampSynchronizationContext>();
        }

        protected virtual void AddNativeEventHandling(IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<NativeTypeConverter>()
                             .AddSingleton<INativeEventRegistry, NativeEventRegistry>();
        }

        protected virtual void AddNativeEvents(IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<INativeEventCollectionFactory, SampEventCollectionFactory>()
                             .AddTransient<INativeEventCollectionFactory, VehiclesEventCollectionFactory>()
                             .AddTransient<INativeEventCollectionFactory, PlayersEventCollectionFactory>()
                             .AddTransient<INativeEventCollectionFactory, ActorEventCollectionFactory>()
                             .AddTransient<INativeEventCollectionFactory, ObjectsEventCollectionFactory>();
        }

        protected virtual void AddNatives(IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<ISampNatives, SampNatives>()
                             .AddTransient<IVehiclesNatives, VehiclesNatives>()
                             .AddTransient<IPlayersNatives, PlayersNatives>()
                             .AddTransient<IActorNatives, ActorNatives>()
                             .AddTransient<IObjectsNatives, ObjectsNatives>();
        }
    }
}
