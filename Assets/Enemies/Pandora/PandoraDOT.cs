using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CombatSystem;

public class PandoraDOT : MonoBehaviour
{
    public float damage;
    
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            combatSystem.LoseHealth(damage);
        }
    }
}
