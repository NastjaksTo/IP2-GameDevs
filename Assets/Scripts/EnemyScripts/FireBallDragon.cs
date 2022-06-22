using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CombatSystem;

public class FireBallDragon : MonoBehaviour
{
    private FatDragonScript parent;
    private float damage;

    private void Awake()
    {
        damage = 40;
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
