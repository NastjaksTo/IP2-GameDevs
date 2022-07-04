using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CombatSystem;
using static CrabAgent_Range;

public class FireBall : MonoBehaviour
{
    private float damage;

    /// <summary>
    /// References set to all necessary Context
    /// </summary>
    private void Awake()
    {
        damage = rangedCrab.FireBallDamage;
    }

    /// <summary>
    /// is called when another Collider is triggering the own Collider
    /// Only fully runned if the other Collider has the Player Tag
    /// </summary>
    /// <param name="other">the colliding Collider</param>
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
