using System.Collections.Generic;
using System.Collections.Immutable;
using JetBrains.Annotations;
using Micky5991.EventAggregator.Interfaces;
using Micky5991.Quests.Entities;
using Micky5991.Quests.Interfaces.Nodes;

namespace Micky5991.Samp.Net.Example.Quests
{
    public abstract class QuestEventTaskNode : QuestTaskNode
    {
        private IImmutableSet<ISubscription> eventSubscriptions = ImmutableHashSet<ISubscription>.Empty;

        protected QuestEventTaskNode([NotNull] IQuestRootNode rootNode)
            : base(rootNode)
        {
        }

        protected abstract IEnumerable<ISubscription> GetEventSubscriptions();

        protected override void AttachEventListeners()
        {
            this.DetachEventListeners();

            this.eventSubscriptions = this.GetEventSubscriptions().ToImmutableHashSet();
        }

        protected override void DetachEventListeners()
        {
            var subscriptions = this.eventSubscriptions;
            this.eventSubscriptions = ImmutableHashSet<ISubscription>.Empty;

            foreach (var subscription in subscriptions)
            {
                subscription.Dispose();
            }
        }
    }
}
