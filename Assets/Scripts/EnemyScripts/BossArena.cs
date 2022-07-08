using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerQuests;

public class BossArena : MonoBehaviour
{
    public static BossArena bossarenaScript;
    
    public GameObject pandoraArenaWall;
    public GameObject pandoraArenaSmoke;
    public GameObject pandoraHealthBar;
    public GameObject pandora;
    public PandoraAgent pandoraAgent;

    public GameObject iceTitanArenaWall;
    public GameObject iceTitanArenaSmoke;
    public GameObject iceTitanHealthBar;
    public GameObject iceTitan;
    public bool isIceTitanAlive;
    
    public GameObject fireTitanArenaWall;
    public GameObject fireTitanArenaSmoke;
    public GameObject fireTitanHealthBar;
    public GameObject fireTitan;
    public bool isFireTitanAlive;
    
    public GameObject earthTitanArenaWall;
    public GameObject earthTitanArenaSmoke;
    public GameObject earthTitanHealthBar;
    public GameObject earthTitan;
    public bool isEarthTitanAlive;
    private bool isQuestCompleted;

    private void Awake()
    {
        isEarthTitanAlive = true;
        isFireTitanAlive = true;
        isIceTitanAlive = true;
        bossarenaScript = this;
        CloseAllArenas();
        ShowAllSmokes();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("PandoraArena"))
        {
            pandoraHealthBar.SetActive(true);
            pandoraArenaWall.SetActive(true);
            pandoraArenaSmoke.SetActive(false);
            pandora.SetActive(true);
            pandoraAgent.ResetRaya();
        }
        if (other.CompareTag("IceArena") && isIceTitanAlive)
        {
            iceTitanArenaWall.SetActive(true);
            iceTitanHealthBar.SetActive(true);
            iceTitanArenaSmoke.SetActive(false);
            iceTitan.SetActive(true);
        }
        if (other.CompareTag("FireArena") && isFireTitanAlive)
        {
            fireTitanArenaWall.SetActive(true);
            fireTitanHealthBar.SetActive(true);
            fireTitanArenaSmoke.SetActive(false);
            fireTitan.SetActive(true);
        }
        if (other.CompareTag("EarthArena") && isEarthTitanAlive)
        {
            earthTitanArenaWall.SetActive(true);
            earthTitanHealthBar.SetActive(true);
            earthTitanArenaSmoke.SetActive(false);
            earthTitan.SetActive(true);
        }
    }

    public void CloseAllArenas()
    {
        pandoraHealthBar.SetActive(false);
        pandoraArenaWall.SetActive(false);
        pandora.SetActive(false);
        earthTitanArenaWall.SetActive(false);
        earthTitanHealthBar.SetActive(false);
        earthTitan.SetActive(false);
        fireTitanArenaWall.SetActive(false);
        fireTitanHealthBar.SetActive(false);
        fireTitan.SetActive(false);
        iceTitanArenaWall.SetActive(false);
        iceTitanHealthBar.SetActive(false);
        iceTitan.SetActive(false);
    }

    public void ShowAllSmokes()
    {
        if(isEarthTitanAlive) earthTitanArenaSmoke.SetActive(true);
        if(isFireTitanAlive) fireTitanArenaSmoke.SetActive(true);
        if(isIceTitanAlive) iceTitanArenaSmoke.SetActive(true);
        pandoraArenaSmoke.SetActive(true);
    }

    public void QuestCompletion()
    {
        if (!isQuestCompleted && !isEarthTitanAlive && !isFireTitanAlive && !isIceTitanAlive)
        {
            isQuestCompleted = true;
            playerQuests.TitanQuest();
        }
    }
}
