using Micky5991.Quests.Interfaces.Nodes;

namespace Micky5991.Samp.Net.Example.Events.EventArgs
{
    public class QuestUpdatedEventArgs : System.EventArgs
    {
        public IQuestNode UpdatedQuest { get; }

        public QuestUpdatedEventArgs(IQuestNode updatedQuest)
        {
            this.UpdatedQuest = updatedQuest;
        }
    }
}
