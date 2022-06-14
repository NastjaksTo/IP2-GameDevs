using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class QuestGoal
{
    public GoalType goalType;

    public int reqAmount;
    public int currentAmount;

    public bool IsReached()
    {
        return currentAmount >= reqAmount;
    }

    public void ObjectFound()
    {
        if (goalType == GoalType.Find) currentAmount++;
    }
}

public enum GoalType
{
    Kill,
    Find
}
