using System;
using System.Collections;
using System.Collections.Generic;
using GameUI;
using StarterAssets;
using UnityEngine;
using UnityEngine.SceneManagement;
using static SkillTree;

public class SaveData : MonoBehaviour
{
    public PlayerSkillsystem skillsystem;
    public PlayerAttributes attributes;
    public ThirdPersonController player;
    public SkillTree skilltree;
    public CombatSystem combatsystem;
    public GameObject playermodel;
    public InventoryObject inventory;
    public InventoryObject equipment;
    public InventoryInterface inf1;
    public InventoryInterface inf2;
    public GameObject scenetransfer;
    public GameObject InventoryUI;
    public UiScreenManager uimanager;
    public bool loaded;

    public int[] skilllevelsData;

    public void Awake()
    {
        scenetransfer = GameObject.FindGameObjectWithTag("SceneTransfer");
        loaded = scenetransfer.GetComponent<SceneTransfer>().loaded;
        if (loaded)
        {
            Invoke("Loadgame", 1f);
        }
        else
        {
            inf1.LoadInterface();
            inf2.LoadInterface();
            inventory.Clear();
            equipment.Clear();
        }
    }



    public void Loadgame()
    {
        Debug.Log("Loading..");
        PlayerData data = SaveSystem.LoadPlayer();

        skillsystem.playerlevel._level = data.level;
        attributes.currentHealth = data.health;
        attributes.staminaRegenerationSpeed = data.staminaregenValue;
        attributes.manaRegenerationSpeed = data.manaregenValue;

        combatsystem.maxpotions = data.maxpotions;
        combatsystem.potions = combatsystem.maxpotions;
        
        for (int i = 0; i <= 17; i++)
        {
            skillTree.skillLevels[i] = data.skilllevels[i];
        }
        skillTree.UpdateAllSkillUI();

        skillTree.healthSkillvalue = data.healthSkillvalue;
        skillTree.manaSkillvalue = data.manaSkillvalue;
        skillTree.staminaSkillvalue = data.staminaSkillvalue;
        

        inf1.LoadInterface();
        inf2.LoadInterface();

        InventoryUI.SetActive(true);
        InventoryUI.SetActive(false);
        
        player.LoadPosition();
        
        inventory.Load();
        equipment.Load();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && CheckpointSystem.checkpointactive)
        {
            Debug.Log("Saving..");
            //SaveSystem.SavePlayer(skillsystem.playerlevel, attributes, skillTree, combatsystem, this);
            
            skilllevelsData = new int[18];
            for (int i = 0; i <= 17; i++)
            {
                skilllevelsData[i] = skillTree.skillLevels[i];
            }

            SaveSystem.SavePlayer(skillsystem.playerlevel, attributes, skillTree, combatsystem, this);

            
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
            
            player.LoadPosition();
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            Debug.Log(skillsystem.playerlevel.getLevel());
        }
    }
}
