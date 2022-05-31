using System;
using System.Collections;
using System.Collections.Generic;
using GameUI;
using StarterAssets;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SaveData : MonoBehaviour
{
    public PlayerSkillsystem skillsystem;
    public PlayerAttributes attributes;
    public ThirdPersonController player;
    public SkillTree skilltree;
    public GameObject playermodel;
    public InventoryObject inventory;
    public InventoryObject equipment;
    public InventoryInterface inf1;
    public InventoryInterface inf2;
    public GameObject scenetransfer;
    public GameObject InventoryUI;
    public GameObject pausemenucontainer;
    public bool loaded;

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

    private void Loadgame()
    {
        Debug.Log("Loading..");
        PlayerData data = SaveSystem.LoadPlayer();

        skillsystem.playerlevel._level = data.level;
        attributes.currentHealth = data.health;
        inf1.LoadInterface();
        inf2.LoadInterface();

        InventoryUI.SetActive(true);
        InventoryUI.SetActive(false);
        
        player.LoadPosition();
        
        inventory.Load();
        equipment.Load();
    }

    private void RefreshUI()
    {
        pausemenucontainer.SetActive(true);
        pausemenucontainer.SetActive(false);
    }

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
