using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CombatSystem;

public class FireBall : MonoBehaviour
{
    private float damage;

    private void Awake()
    {
        damage = 20;
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
