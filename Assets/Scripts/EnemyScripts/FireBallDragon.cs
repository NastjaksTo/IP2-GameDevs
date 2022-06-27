using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CombatSystem;
using static FatDragonScript;

public class FireBallDragon : MonoBehaviour
{
    private float damage;

    private void Awake()
    {
        damage = dragonBoss.FireBallDamage;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            combatSystem.LoseHealth(damage);
            Destroy(gameObject);
        }
        Destroy(gameObject, 5);
    }
}
