using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestGiver : MonoBehaviour
{
    public QuestSystem quest;
    public PlayerQuests player;

    public void AcceptQuest()
    {
        player.quest = quest;
    }
}
