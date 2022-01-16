using Micky5991.EventAggregator.Elements;
using Micky5991.Quests.Interfaces.Nodes;
using Micky5991.Samp.Net.Framework.Interfaces.Entities;

namespace Micky5991.Samp.Net.Example.Events
{
    public class PlayerQuestReceived : EventBase
    {
        public IPlayer Player { get; }

        public IQuestRootNode Quest { get; }

        public PlayerQuestReceived(IPlayer player, IQuestRootNode quest)
        {
            this.Player = player;
            this.Quest = quest;
        }
    }
}
