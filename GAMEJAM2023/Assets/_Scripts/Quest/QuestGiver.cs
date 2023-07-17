using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestGiver : NPC
{
    public bool AssignedQuest { get; set; }
    public bool Helped { get; set; }
    public Quest Quest { get; set; }

    [SerializeField]
    private GameObject quests;
    [SerializeField]
    private string questType;

    public override void Interact()
    {
        if (!AssignedQuest && !Helped)
        {
            // Dialogue
            base.Interact();
            Debug.Log("Here's your quest");
            AssignQuest();
        }
        else if (AssignedQuest && !Helped)
        {
            CheckQuestCompletion();
        }
        else
        {
            // Dialogue quest completed 
            Debug.Log("Thanks for completing the quest");
        }

    }

    void AssignQuest()
    {
        AssignedQuest = true;
        Quest = (Quest)quests.AddComponent(System.Type.GetType(questType));

    }

    void CheckQuestCompletion()
    {
        if (Quest.Completed)
        {
            Quest.GiveReward();
            Helped = true;
            AssignedQuest = false;
            // Dialogue
            Debug.Log("Quest completed");
        }
        else
        {   // Dialogue
            Debug.Log("Quest in progress");
        }
    }

    public override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
    }
}
