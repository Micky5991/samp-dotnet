using Micky5991.EventAggregator.Interfaces;
using Micky5991.Quests.Entities;
using Micky5991.Samp.Net.Core.Natives.Samp;
using Micky5991.Samp.Net.Example.Quests.Tasks;
using Micky5991.Samp.Net.Framework.Interfaces;

namespace Micky5991.Samp.Net.Example.Quests
{
    public class WelcomeQuest : QuestRootNode
    {
        private readonly IVehicleMeta vehicleMeta;

        private readonly IEventAggregator eventAggregator;

        public WelcomeQuest(IVehicleMeta vehicleMeta, IEventAggregator eventAggregator)
        {
            this.vehicleMeta = vehicleMeta;
            this.eventAggregator = eventAggregator;

            this.Title = "Welcome Home";

            this.SetChildQuests(new QuestSequenceNode(this)
            {
                new EnterCarTask(Vehicle.Bullet, this, this.vehicleMeta, this.eventAggregator),
                new EnterCarTask(Vehicle.Bmx, this, this.vehicleMeta, this.eventAggregator),
                new EnterCarTask(Vehicle.Taxi, this, this.vehicleMeta, this.eventAggregator),
                new EnterCarTask(Vehicle.Policecarlspd, this, this.vehicleMeta, this.eventAggregator),

                new QuestParallelNode(this)
                {
                    new StayAliveTask(this),
                    new EnterCarTask(Vehicle.Ambulance, this, this.vehicleMeta, this.eventAggregator),
                    new EnterCarTask(Vehicle.Nrg500, this, this.vehicleMeta, this.eventAggregator),
                },

                new QuestParallelNode(this)
                {
                    new EnterCarTask(Vehicle.Comet, this, this.vehicleMeta, this.eventAggregator),
                    new EnterCarTask(Vehicle.Bus, this, this.vehicleMeta, this.eventAggregator),

                    new QuestAnySuccessSequenceNode(this)
                    {
                        new EnterCarTask(Vehicle.Andromada, this, this.vehicleMeta, this.eventAggregator),
                        new EnterCarTask(Vehicle.Raindance, this, this.vehicleMeta, this.eventAggregator),
                    },
                },
            });
        }
    }
}
