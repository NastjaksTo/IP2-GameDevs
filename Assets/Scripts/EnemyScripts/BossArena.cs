using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossArena : MonoBehaviour
{
    public static BossArena bossarenaScript;
    
    public GameObject pandoraArenaWall;
    public GameObject pandoraHealthBar;
    public GameObject pandora;
    
    public GameObject iceTitanArenaWall;
    public GameObject iceTitanHealthBar;
    public GameObject iceTitan;
    public bool isIceTitanAlive;
    
    public GameObject fireTitanArenaWall;
    public GameObject fireTitanHealthBar;
    public GameObject fireTitan;
    public bool isFireTitanAlive;
    
    public GameObject earthTitanArenaWall;
    public GameObject earthTitanHealthBar;
    public GameObject earthTitan;
    public bool isEarthTitanAlive;

    private void Awake()
    {
        isEarthTitanAlive = true;
        isFireTitanAlive = true;
        isIceTitanAlive = true;
        bossarenaScript = this;
        CloseAllArenas();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("PandoraArena"))
        {
            pandoraHealthBar.SetActive(true);
            pandoraArenaWall.SetActive(true);
            pandora.SetActive(true);
        }
        if (other.CompareTag("IceArena") && isIceTitanAlive)
        {
            iceTitanArenaWall.SetActive(true);
            iceTitanHealthBar.SetActive(true);
            iceTitan.SetActive(true);
        }
        if (other.CompareTag("FireArena") && isFireTitanAlive)
        {
            fireTitanArenaWall.SetActive(true);
            fireTitanHealthBar.SetActive(true);
            fireTitan.SetActive(true);
        }
        if (other.CompareTag("EarthArena") && isEarthTitanAlive)
        {
            earthTitanArenaWall.SetActive(true);
            earthTitanHealthBar.SetActive(true);
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
}
