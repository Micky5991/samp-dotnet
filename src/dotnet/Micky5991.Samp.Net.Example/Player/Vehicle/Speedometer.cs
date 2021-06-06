using System;
using System.Drawing;
using System.Timers;
using Micky5991.EventAggregator.Interfaces;
using Micky5991.Samp.Net.Core.Interfaces.Interop;
using Micky5991.Samp.Net.Framework.Events.Samp;
using Micky5991.Samp.Net.Framework.Interfaces;
using Micky5991.Samp.Net.Framework.Interfaces.Entities.Factories;
using Microsoft.Extensions.Logging;

namespace Micky5991.Samp.Net.Example.Player.Vehicle
{
    public class Speedometer : IEntityListener
    {
        private readonly IEventAggregator eventAggregator;

        private readonly ILogger<Speedometer> logger;

        private readonly ISampThreadEnforcer threadEnforcer;

        private readonly IMainTimerFactory mainTimerFactory;

        public Speedometer(IEventAggregator eventAggregator, ILogger<Speedometer> logger, ISampThreadEnforcer threadEnforcer, IMainTimerFactory mainTimerFactory)
        {
            this.eventAggregator = eventAggregator;
            this.logger = logger;
            this.threadEnforcer = threadEnforcer;
            this.mainTimerFactory = mainTimerFactory;
        }

        public void Attach()
        {
            this.eventAggregator.Subscribe<PlayerEnterVehicleEvent>(this.OnPlayerEnterVehicle);

            var timer = this.mainTimerFactory.CreateTimer(TimeSpan.FromSeconds(1));
            timer.Elapsed += this.OnTimerElapsed;
            timer.Start();

        }

        private void OnTimerElapsed(object sender, ElapsedEventArgs e)
        {
            this.logger.LogInformation("Timer elapsed");
            this.threadEnforcer.EnforceMainThread();
        }

        private void OnPlayerEnterVehicle(PlayerEnterVehicleEvent eventdata)
        {
            eventdata.Player.SendMessage(Color.GreenYellow, "Enter");
        }
    }
}