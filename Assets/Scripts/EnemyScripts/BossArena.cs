using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossArena : MonoBehaviour
{
    public GameObject pandoraArenaWall;
    public GameObject pandoraHealthBar;
    public GameObject pandora;
    

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("PandoraArena"))
        {
            pandoraHealthBar.SetActive(true);
            pandoraArenaWall.SetActive(true);
            pandora.SetActive(true);
        }
    }

    public void CloseAllGameObjects()
    {
        pandoraHealthBar.SetActive(false);
        pandoraArenaWall.SetActive(false);
        pandora.SetActive(false);
    }
}
