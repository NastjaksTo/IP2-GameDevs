using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonSword : MonoBehaviour
{
    private bool isAbleToAttack;                    // Boolean inorder to check wether or not the Weapon is able to deal damage.
    private GameObject target;                      // Reference to the GameObject currently getting attacked.
    private SkeletonAgent skeletonAgent;            // Reference to the SkeletonAgent script.
    
    /// <summary>
    /// Gets the player gameobject.
    /// Gets the skeletonagent script.
    /// </summary>
    private void Awake()
    {
        target = GameObject.FindWithTag("Player");
        skeletonAgent = GetComponentInParent<SkeletonAgent>();
    }

    /// <summary>
    /// Compare the tag of the GameObject currently colliding with the weapon. If its the player, deal damage to him.
    /// </summary>
    /// <param name="other">other is a variable saving the GameObject which is colliding with the weapon</param>
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && skeletonAgent.isAttacking)
        {
            target.GetComponent<CombatSystem>().LoseHealth(skeletonAgent.sworddamage);
        }
    }
}
