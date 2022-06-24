using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerQuests;
using static PlayerSkillsystem;

[System.Serializable]
public class QuestSystem
{
    public string title;
    [TextArea(5, 20)]public string descrption;
    public int expReward;

    public void Complete()
    {
        playerskillsystem.playerlevel.AddExp(expReward);
    }
}
