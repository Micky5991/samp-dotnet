using System;
using System.Collections.Generic;
using Micky5991.EventAggregator.Interfaces;
using Micky5991.Quests.Interfaces.Nodes;
using Micky5991.Samp.Net.Core.Natives.Samp;
using Micky5991.Samp.Net.Example.Quests.Bases;
using Micky5991.Samp.Net.Framework.Events.Samp;
using Micky5991.Samp.Net.Framework.Interfaces;

namespace Micky5991.Samp.Net.Example.Quests.Tasks
{
    public class EnterCarTask : QuestEventTaskNode
    {
        private readonly Vehicle vehicleModel;

        private readonly IEventAggregator eventAggregator;

        public EnterCarTask(Vehicle vehicleModel, IQuestRootNode rootNode, IVehicleMeta vehicleMeta, IEventAggregator eventAggregator)
            : base(rootNode)
        {
            this.vehicleModel = vehicleModel;
            this.eventAggregator = eventAggregator;

            this.Title = $"Find and enter a ~g~~h~~h~{vehicleMeta.GetVehicleName(vehicleModel)}~s~.";
        }

        protected override IEnumerable<ISubscription> GetEventSubscriptions()
        {
            yield return this.eventAggregator.Subscribe<PlayerEnterVehicleEvent>(this.OnPlayerEnterVehicle);
        }

        private void OnPlayerEnterVehicle(PlayerEnterVehicleEvent eventdata)
        {
            if (eventdata.Vehicle.Model == this.vehicleModel)
            {
                this.MarkAsSuccess();
            }
        }
    }
}
