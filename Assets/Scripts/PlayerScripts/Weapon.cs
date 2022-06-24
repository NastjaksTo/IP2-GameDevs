using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CombatSystem;
using static PlayerAttributes;
public class Weapon : MonoBehaviour
{
    private bool isAbleToAttack;                    // Boolean inorder to check wether or not the Weapon is able to deal damage.
    private GameObject currentGo;                   // Reference to the GameObject currently getting attacked.

    /// <summary>
    /// Compare the tag of the GameObject currently colliding with the weapon. If its an enemy, deal damage to that GameObject.
    /// </summary>
    /// <param name="other">other is a variable saving the GameObject which is colliding with the weapon</param>
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Enemy") && combatSystem.isAttacking)
        {
            currentGo = other.gameObject;
            Debug.Log("hitting");
            currentGo.GetComponent<EnemyHealthHandler>().getDamage((int)playerAttributesScript.physicalDamage);
        }
    }
}
