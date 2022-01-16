using System.Collections.Generic;
using JetBrains.Annotations;
using Micky5991.EventAggregator.Interfaces;
using Micky5991.Quests.Interfaces.Nodes;
using Micky5991.Samp.Net.Example.Quests.Bases;

namespace Micky5991.Samp.Net.Example.Quests.Tasks
{
    public class StayAliveTask : QuestEventConditionTaskNode
    {
        public StayAliveTask([NotNull] IQuestRootNode rootNode)
            : base(rootNode)
        {
            this.Title = "Do not die!";
        }

        protected override IEnumerable<ISubscription> GetEventSubscriptions()
        {
            yield break;
        }
    }
}
