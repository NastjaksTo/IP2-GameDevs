using System;
using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using UnityEngine;

public class SaveData : MonoBehaviour
{
    public PlayerSkillsystem skillsystem;
    public PlayerAttributes attributes;
    public ThirdPersonController player;
    public SkillTree skilltree;
    public GameObject playermodel;
    public InventoryObject inventory;
    public InventoryObject equipment;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            Debug.Log("Saving..");
            SaveSystem.SavePlayer(skillsystem.playerlevel, attributes, this);
            inventory.Save();
            equipment.Save();
        }


        if (Input.GetKeyDown(KeyCode.L))
        {
            Debug.Log("Loading..");
            PlayerData data = SaveSystem.LoadPlayer();

            skillsystem.playerlevel._level = data.level;
            attributes.currentHealth = data.health;
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            Debug.Log(skillsystem.playerlevel.getLevel());
        }
    }
}
