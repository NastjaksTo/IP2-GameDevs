using System;
using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using UnityEngine;

public class SaveData : MonoBehaviour
{
    public PlayerSkillsystem skillsystem;
    public PlayerAttributes attributes;
    public InventoryObject inventory;
    public InventoryObject equipment;
    public InventoryInterface inf1;
    public InventoryInterface inf2;
    public ThirdPersonController player;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            Debug.Log("Saving..");
            
            SaveSystem.SavePlayer(skillsystem, attributes, this);
            inventory.Save();
            equipment.Save();
        }


        if (Input.GetKeyDown(KeyCode.L))
        {
            Debug.Log("Loading..");
            PlayerData data = SaveSystem.LoadPlayer();
            
            skillsystem.playerlevel._level = data.level;
            attributes.currentHealth = data.health;
            inf1.LoadInterface();
            inf2.LoadInterface();

            inventory.Load();
            equipment.Load();
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            Debug.Log(skillsystem.playerlevel.getLevel());
        }
    }

    public void DataLoad()
    {
        Debug.Log("Loading..");
        PlayerData data = SaveSystem.LoadPlayer();

        skillsystem.playerlevel._level = data.level;
        attributes.currentHealth = data.health;
        inf1.LoadInterface();
        inf2.LoadInterface();

        inventory.Load();
        equipment.Load();
        
        player.PositionLoad();
    }
}
