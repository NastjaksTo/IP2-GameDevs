using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using UnityEngine;
using static SkillTree;

[System.Serializable]
public class PlayerData
{
    public int level;
    public int health;
    public float[] position;
    public int[] skilllevels;

    public PlayerData(LevelSystem levelsystem, PlayerAttributes attributes, SkillTree skillTree, SaveData player)
    {
        level = PlayerSkillsystem.playerskillsystem.playerlevel.getLevel();
        health = attributes.currentHealth;
        
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
