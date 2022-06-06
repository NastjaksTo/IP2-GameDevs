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
        skillsystem.playerlevel._exp = data.currentExp;
        skillsystem.playerlevel._expToLevelUp = data.EXPtoLvlUP;
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

        combatsystem.refillPotions();
        skillsystem.updateLevelUI();
    
        inventory.Load();
        equipment.Load();

        gameObject.GetComponent<CharacterController>().enabled = false;
        Vector3 position;
        position.x = data.position[0];
        position.y = data.position[1];
        position.z = data.position[2];
        transform.position = position;
        gameObject.GetComponent<CharacterController>().enabled = true;

    }

    public void saveGame() {
        Debug.Log("Saving..");

        skilllevelsData = new int[18];
        for (int i = 0; i <= 17; i++) {
            skilllevelsData[i] = skillTree.skillLevels[i];
        }

        SaveSystem.SavePlayer(skillsystem.playerlevel, attributes, skillTree, combatsystem, this);

        inventory.Save();
        equipment.Save();
    }

    private void Update() {

        if (Input.GetKeyDown(KeyCode.E) && CheckpointSystem.checkpointactive) {
            if (UiScreenManager._skillUiOpen) {
                saveGame();
                combatsystem.refillPotions();
                uimanager.CloseSkillUi();
            } else {
                saveGame();
                combatsystem.refillPotions();
                uimanager.OpenSkillUi();
            }
        }
    }

}
