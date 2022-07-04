using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CombatSystem;

public class PandoraDOT : MonoBehaviour
{
    public float damage;                            // Float to save the damage the spell does.
    
    /// <summary>
    /// Deal damage to the player each frame the player is inside the collider of this gameobject.
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            combatSystem.LoseHealth(damage);
        }
    }
}
