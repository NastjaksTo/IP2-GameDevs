using System;
using System.Collections;
using System.Collections.Generic;
using GameUI;
using StarterAssets;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using static SkillTree;


public class SaveData : MonoBehaviour
{
    public PlayerSkillsystem skillsystem;           // Reference to the PlayerSkillsystem script.
    public PlayerAttributes attributes;             // Reference to the PlayerAttributes script.
    public CombatSystem combatsystem;               // Reference to the CombatSystem script.
    public InventoryObject inventory;               // Reference to the InventoryObject script attached to the inventory.
    public InventoryObject equipment;               // Reference to the InventoryObject script attached to the equipment.
    public InventoryInterface inf1;                 // Reference to the InventoryInterface script attached to the inventory.
    public InventoryInterface inf2;                 // Reference to the InventoryInterface script attached to the equipment.
    public UiScreenManager uimanager;               // Reference to the UiScreenManager script.
    
    public GameObject scenetransfer;                // Reference to the Scenetransfer GameObject which holds information on whether or not to load the game.
    public GameObject inventoryUI;                  // Reference to the inventory UI.

    public bool loaded;                             // Boolean which decides whether or not to load the game.
    public int[] skilllevelsData;                   // Integer array which holds the skill levels.

    /// <summary>
    /// When the script instance is loaded, get the scenetransfer gameobject, assign the loaded boolean and load the game if loaded is true.
    /// Otherwise clear the inventory and the equipment.
    /// </summary>
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

    /// <summary>
    /// Calls the load method from SaveSystem script. Updates all values according to the loaded data.
    /// </summary>
    public void Loadgame()
    {
        Debug.Log("Loading..");
        PlayerData data = SaveSystem.LoadPlayer();

        skillsystem.playerlevel.level = data.level;
        skillsystem.playerlevel.exp = data.currentExp;
        skillsystem.playerlevel.expToLevelUp = data.expToLvlUp;
        attributes.currentHealth = data.health;
        attributes.staminaRegenerationSpeed = data.staminaregenValue;
        attributes.manaRegenerationSpeed = data.manaregenValue;

        combatsystem.maxpotions = data.maxpotions;
        combatsystem.potions = combatsystem.maxpotions;
        
        for (int i = 0; i <= 17; i++)
        {
            skillTree.skillLevels[i] = data.skilllevels[i];
        }
        
        skillTree.healthSkillvalue = data.healthSkillvalue;
        skillTree.manaSkillvalue = data.manaSkillvalue;
        skillTree.staminaSkillvalue = data.staminaSkillvalue;
        
        inf1.LoadInterface();
        inf2.LoadInterface();
        inventoryUI.SetActive(true);
        inventoryUI.SetActive(false);
        
        inventory.Load();
        equipment.Load();
        
        combatsystem.refillPotions();
        skillsystem.updateLevelUI();
        skillTree.UpdateAllSkillUI();
        
        gameObject.GetComponent<CharacterController>().enabled = false;
        Vector3 position;
        position.x = data.position[0];
        position.y = data.position[1];
        position.z = data.position[2];
        transform.position = position;
        gameObject.GetComponent<CharacterController>().enabled = true;
    }

    /// <summary>
    /// Calls the save method from SaveSystem, which saves all data in binary files.
    /// </summary>
    public void SaveGame() {
        Debug.Log("Saving..");
        skilllevelsData = new int[18];
        for (int i = 0; i <= 17; i++) {
            skilllevelsData[i] = skillTree.skillLevels[i];
        }
        SaveSystem.SavePlayer(skillsystem.playerlevel, attributes, skillTree, combatsystem, this);
        inventory.Save();
        equipment.Save();
    }

    /// <summary>
    /// Whenever the player reaches an checkpoint and presses "E" the SkillTree interface opens and the game is saved.
    /// </summary>
    private void Update() 
    {
        if (Input.GetKeyDown(KeyCode.E) && CheckpointSystem.checkpointactive) 
        {
            if (UiScreenManager._skillUiOpen) 
            {
                SaveGame();
                combatsystem.refillPotions();
                uimanager.CloseSkillUi();
            } else 
            {
                SaveGame(); 
                combatsystem.refillPotions();
                uimanager.OpenSkillUi();
            }
        }
    }
}
