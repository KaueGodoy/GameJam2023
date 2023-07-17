using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestTest : Quest
{
    private void Start()
    {
        QuestName = "Quest test";
        Description = "Kill some monsters";
        ItemReward = ItemDatabase.Instance.GetItem("potion_log");

        Goals.Add(new KillGoal(this, 0, "Kill 2 beetles", false, 0, 2));
        Goals.Add(new KillGoal(this, 1, "Kill 1 slime", false, 0, 1));
        Goals.Add(new CollectionGoal(this, "coin", "Collect 2 coins", false, 0, 2));

        Goals.ForEach(g => g.Init());
    }
}
