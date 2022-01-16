using System.Collections.Generic;
using Micky5991.Quests.Entities;
using Micky5991.Samp.Net.Example.Quests;

namespace Micky5991.Samp.Net.Example.Services
{
    public class QuestRegistry : Micky5991.Quests.Services.QuestRegistry
    {
        protected override IEnumerable<QuestMeta> BuildAvailableQuestMeta()
        {
            yield return this.BuildQuest<WelcomeQuest>();
        }
    }
}
