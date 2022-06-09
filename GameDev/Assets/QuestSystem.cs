using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerQuests;

[System.Serializable]
public class QuestSystem
{
    public bool isActive;
    public string title;
    public string descrption;
    public int expReward;

    public QuestGoal goal;

    public void Complete()
    {
        isActive = false;
        Debug.Log(title + " is completed");
        playerQuests.titleText.text = "";
        playerQuests.descText.text = "Hier könnte ein Tipp stehen, der uns zur nächsten Quest führt.";
        playerQuests.rewardText.text = "0";
    }
}
