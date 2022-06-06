using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using UnityEngine;
using static SkillTree;

[System.Serializable]
public class PlayerData
{
    public int level;
    public float health;
    public float[] position;
    public int[] skilllevels;
    public int healthSkillvalue;
    public int manaSkillvalue;
    public int staminaSkillvalue;
    public float manaregenValue;
    public float staminaregenValue;
    public int maxpotions;
    
    public PlayerData(LevelSystem levelsystem, PlayerAttributes attributes, SkillTree skillTree, CombatSystem combatSystem, SaveData player)
    {
        level = PlayerSkillsystem.playerskillsystem.playerlevel.getLevel();
        health = attributes.currentHealth;
        healthSkillvalue = skillTree.healthSkillvalue;
        manaSkillvalue = skillTree.manaSkillvalue;
        staminaSkillvalue = skillTree.staminaSkillvalue;
        manaregenValue = attributes.manaRegenerationSpeed;
        staminaregenValue = attributes.staminaRegenerationSpeed;
        maxpotions = combatSystem.maxpotions;
        
        skilllevels = new int[18];
        for (int i = 0; i <= 17; i++)
        {
            skilllevels[i] = skillTree.skillLevels[i];
        }

        
        position = new float[3];
        position[0] = player.transform.position.x;
        position[1] = player.transform.position.y;
        position[2] = player.transform.position.z;
    }
}
