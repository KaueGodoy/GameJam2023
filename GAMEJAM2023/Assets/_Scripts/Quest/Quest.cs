using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Quest : MonoBehaviour
{
    public List<Goal> Goals { get; set; } = new List<Goal>();
    public string QuestName { get; set; }
    public string Description { get; set; }
    //public int ExperienceReward { get; set; }
    public Item ItemReward { get; set; }
    public bool Completed { get; set; }

    public void CheckGoals()
    {
        // if all goals in the goal list are completed return quest completed bool true
        Completed = Goals.All(g => g.Completed);

        //if (Completed) GiveReward();
    }

    public void GiveReward()
    {
        if (ItemReward != null)
        {
            InventoryController.Instance.GiveItem(ItemReward);
        }
    }
}
