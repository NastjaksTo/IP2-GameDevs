using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    private bool isAbleToAttack;                    // Boolean inorder to check wether or not the Weapon is able to deal damage.
    private GameObject currentGo;                   // Reference to the GameObject currently getting attacked.
    private PlayerAttributes playerattributes;      // Reference to the players attributes.

    /// <summary>
    /// Get the reference to the players attributes when the weapon object is instantiated.
    /// </summary>
    private void Awake()
    {
        playerattributes = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerAttributes>();
    }

    /// <summary>
    /// Compare the tag of the GameObject currently colliding with the weapon. If its an enemy, deal damage to that GameObject.
    /// </summary>
    /// <param name="other">other is a variable saving the GameObject which is colliding with the weapon</param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            currentGo = other.gameObject;
            Debug.Log("hitting");
        }
    }
}
