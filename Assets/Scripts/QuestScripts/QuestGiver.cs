using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class QuestGiver : MonoBehaviour
{
    public QuestSystem quest;               // Reference to the QuestSystem script.
    public PlayerQuests player;             // Reference to the PlayerQuests script.
    
    /// <summary>
    /// Accepts the quest and assigns the values of this new quest to the players quest.
    /// </summary>
    public void AcceptQuest()
    {
        player.quest = quest;
    }
}

