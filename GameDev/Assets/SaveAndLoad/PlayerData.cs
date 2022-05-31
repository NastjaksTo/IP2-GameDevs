using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public int level;
    public int health;
    public float[] position;

    public PlayerData(PlayerSkillsystem playerskillsystem, PlayerAttributes attributes, SaveData player)
    {
        level = playerskillsystem.playerlevel.getLevel();
        health = attributes.currentHealth;
        
        position = new float[3];
        position[0] = player.transform.position.x;
        position[1] = player.transform.position.y;
        position[2] = player.transform.position.z;
    }
}
