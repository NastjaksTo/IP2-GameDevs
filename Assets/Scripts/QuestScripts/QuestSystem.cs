using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerQuests;
using static PlayerSkillsystem;

[System.Serializable]
public class QuestSystem
{
    public string title;                            // String for the quest title.
    [TextArea(5, 20)]public string descrption;      // String for the quest description.
    public int expReward;                           // Integer for the quest reward.
    public int questID;                             // Integer for the quest id.

    /// <summary>
    /// Completes the quest the player currently has active.
    /// </summary>
    public void Complete()
    {
        playerskillsystem.playerlevel.AddExp(expReward);
    }
}
