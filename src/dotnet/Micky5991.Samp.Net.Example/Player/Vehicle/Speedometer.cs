using System;
using System.Drawing;
using System.Numerics;
using System.Timers;
using Micky5991.EventAggregator.Interfaces;
using Micky5991.Samp.Net.Core.Interfaces.Interop;
using Micky5991.Samp.Net.Framework.Elements.TextDraws;
using Micky5991.Samp.Net.Framework.Enums;
using Micky5991.Samp.Net.Framework.Events.Samp;
using Micky5991.Samp.Net.Framework.Extensions;
using Micky5991.Samp.Net.Framework.Interfaces;
using Micky5991.Samp.Net.Framework.Interfaces.Entities.Factories;
using Micky5991.Samp.Net.Framework.Interfaces.TextDraws;
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
            this.eventAggregator.Subscribe<PlayerExitVehicleEvent>(this.OnPlayerExitVehicle);

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

            if (eventdata.Vehicle.TryGetData<ITextDraw>("speedometer", out var speedometer) == false)
            {
                speedometer = new TextDraw(
                                           new Vector2(505, 345),
                                           "speedometer")
                {
                    UseBox = true,
                    TextFont = TextFont.AharoniBold,
                    LetterSize = new Vector2(0.2f, 0.9f),
                    OutlineSize = 1,
                    BackgroundColor = Color.Black,
                    TextColor = Color.White,
                    BoxColor = Color.Black.Transparentize(.5f),
                    TextSize = new Vector2(600, 500),
                };

                eventdata.Vehicle.SetData("speedometer", speedometer);
            }

            eventdata.Player.ShowTextDraw(speedometer);
        }

        private void OnPlayerExitVehicle(PlayerExitVehicleEvent eventdata)
        {
            eventdata.Player.SendMessage(Color.GreenYellow, "Exit");

            if (eventdata.Vehicle.TryGetData<ITextDraw>("speedometer", out var speedometer))
            {
                eventdata.Player.HideTextDraw(speedometer);
            }
        }
    }
}
