using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Timers;
using Micky5991.EventAggregator.Interfaces;
using Micky5991.Samp.Net.Core.Natives.Samp;
using Micky5991.Samp.Net.Framework.Elements.TextDraws;
using Micky5991.Samp.Net.Framework.Enums;
using Micky5991.Samp.Net.Framework.Events.Samp;
using Micky5991.Samp.Net.Framework.Extensions;
using Micky5991.Samp.Net.Framework.Interfaces;
using Micky5991.Samp.Net.Framework.Interfaces.Entities;
using Micky5991.Samp.Net.Framework.Interfaces.Entities.Factories;
using Micky5991.Samp.Net.Framework.Interfaces.Entities.Pools;
using Micky5991.Samp.Net.Framework.Interfaces.TextDraws;

namespace Micky5991.Samp.Net.Example.Player.Vehicle
{
    public class Speedometer : IEntityListener
    {
        private const bool UseMetricSystem = true;

        private const double DistanceFactor = UseMetricSystem ? 1.609344 : 1;

        private const string DistanceUnit = UseMetricSystem ? "km" : "mi";

        private const string SpeedUnit = DistanceUnit + "/h";

        private readonly IEventAggregator eventAggregator;

        private readonly IMainTimerFactory timerFactory;

        private readonly IVehiclePool vehiclePool;

        private readonly IVehicleMeta vehicleMeta;

        private IMainTimer timer;

        public Speedometer(
            IEventAggregator eventAggregator,
            IMainTimerFactory timerFactory,
            IVehiclePool vehiclePool,
            IVehicleMeta vehicleMeta)
        {
            this.eventAggregator = eventAggregator;
            this.timerFactory = timerFactory;
            this.vehiclePool = vehiclePool;
            this.vehicleMeta = vehicleMeta;
        }

        public void Attach()
        {
            this.eventAggregator.Subscribe<PlayerStateChangeEvent>(this.OnPlayerStateChanged);

            this.timer = this.timerFactory.CreateTimer(TimeSpan.FromSeconds(0.25));
            this.timer.Elapsed += this.OnTimerTick;
            this.timer.Start();
        }

        private void OnTimerTick(object sender, ElapsedEventArgs e)
        {
            foreach (var vehicle in this.vehiclePool.Entities.Values)
            {
                if (vehicle.Disposed)
                {
                    continue;
                }

                this.UpdateDistance(vehicle);
                this.UpdateSpeedometer(vehicle);
            }
        }

        private void OnPlayerStateChanged(PlayerStateChangeEvent eventdata)
        {
            if (eventdata.NewState is PlayerState.Driver or PlayerState.Passenger)
            {
                this.ShowSpeedometer(eventdata.Player);

                return;
            }

            this.HideSpeedometer(eventdata.Player);
        }

        private void ShowSpeedometer(IPlayer player)
        {
            if (player.VehicleId == null)
            {
                return;
            }

            var vehicle = this.vehiclePool.FindOrDefaultEntity(player.VehicleId.Value);
            if (vehicle == null)
            {
                return;
            }

            if (vehicle.TryGetData<List<ITextDraw>>("speedometer", out var speedometerParts) == false)
            {
                speedometerParts = new List<ITextDraw>
                {
                    new TextDraw(
                                 new Vector2(500, 380),
                                 "")
                    {
                        UseBox = true,
                        TextFont = TextFont.AharoniBold,
                        LetterSize = new Vector2(0.2f, 0.9f),
                        OutlineSize = 1,
                        BackgroundColor = Color.Black,
                        TextColor = Color.White,
                        BoxColor = Color.Black.Transparentize(.5f),
                        TextSize = new Vector2(600, 500),
                    },
                    new TextDraw(
                                 new Vector2(500, 367),
                                 this.vehicleMeta.GetVehicleName(vehicle.Model))
                    {
                        TextFont = TextFont.BeckettRegular,
                        LetterSize = new Vector2(0.45f, 1.8f),
                        OutlineSize = 0,
                        TextColor = Color.White,
                        BackgroundColor = Color.Black,
                        TextSize = new Vector2(620, 300),
                    }
                };


                vehicle.SetData("speedometer", speedometerParts);
                player.SetData("speedometer", speedometerParts);
            }

            foreach (var speedometerPart in speedometerParts)
            {
                player.ShowTextDraw(speedometerPart);
            }
        }

        private void HideSpeedometer(IPlayer player)
        {
            if (player.TryGetData<List<ITextDraw>>("speedometer", out var speedometers) == false)
            {
                return;
            }

            foreach (var speedometer in speedometers)
            {
                player.HideTextDraw(speedometer);
            }
        }

        private void UpdateSpeedometer(IVehicle vehicle)
        {
            if (vehicle.TryGetData("speedometer", out IList<ITextDraw> speedometers) == false)
            {
                return;
            }

            if (vehicle.TryGetData("distance", out double distance) == false)
            {
                distance = 0;
            }

            var speedometer = speedometers.First();

            distance = Math.Max(0, distance);
            var speed = Math.Max(0, vehicle.Velocity.Length() * 100 * DistanceFactor);
            var health = Math.Max(0, (vehicle.Health - 250) / 750);
            var convertedDistance = distance / 1000 * (1 / DistanceFactor);

            speedometer.Text = string.Join(
                                           "~n~",
                                           "",
                                           $"~g~~h~ Speed: ~s~{Math.Floor(speed):F0} {SpeedUnit}",
                                           $"~r~ Health: ~s~{health * 100:F1}%",
                                           $"~s~ Distance: {convertedDistance:N2} {DistanceUnit}");
        }

        private void UpdateDistance(IVehicle vehicle)
        {
            if (vehicle.TryGetData("distance", out double distance) == false)
            {
                distance = 0;
            }

            if (vehicle.TryGetData("distance_position", out Vector3 lastPosition) == false)
            {
                vehicle.SetData("distance_position", vehicle.Position);

                return;
            }

            var currentPosition = vehicle.Position;

            distance += (currentPosition - lastPosition).Length();

            vehicle.SetData("distance_position", currentPosition);
            vehicle.SetData("distance", distance);
        }
    }
}
