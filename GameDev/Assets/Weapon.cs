using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    private bool isabletoAttack;
    private GameObject currentGO;
    private PlayerAttributes playerattributes;

    private void Awake()
    {
        playerattributes = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerAttributes>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            currentGO = other.gameObject;
            Debug.Log("hitting");
        }
    }
}
