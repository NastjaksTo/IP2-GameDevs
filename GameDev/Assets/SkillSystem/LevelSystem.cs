using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using static SkillTree;
using static PlayerSkillsystem;

public class LevelSystem
{
    private int _level;
    private float _exp;
    private float _expToLevelUp;
    public int skillpoints;

    public LevelSystem() // Setting standard variables to LevelSystem
    {
        _level = 1;
        _exp = 0;
        skillpoints = 0;
        _expToLevelUp = 1;
    }

    public void AddExp(int amount) // Gain experience and level up
    {
        _exp += amount;

        if (_exp > _expToLevelUp)
        {
            _level++;
            skillpoints++;
            _exp -= _expToLevelUp;
            _expToLevelUp += _expToLevelUp;
            playerskillsystem.PlayLvlUpEffect();
            skillTree.UpdateAllSkillUI();
        }
    }

    public int getLevel() // Return current level
    {
        return _level;
    }

    public int getSP() // Return current skillpoints
    {
        return skillpoints;
    }


    public float getExpToLevelUp() // Return currently needed expierence in order to level up
    {
        return _expToLevelUp-_exp;
    }
    public float getExp() // Return currently needed expierence in order to level up
   {
        return _exp;
    }

}
