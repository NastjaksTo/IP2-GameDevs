using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static BossGolemFire;
using static CombatSystem;

public class FireBallTitan : MonoBehaviour
{
    private float damage;

    private void Awake()
    {
        damage = fireTitan.FireDamage;
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
