using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CombatSystem;

public class PandoraDamage : MonoBehaviour
{
    public float damage;

    private void Awake()
    {
        Destroy(gameObject.GetComponent<BoxCollider>(), 0.5f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            combatSystem.LoseHealth(damage);
        }
    }

}
