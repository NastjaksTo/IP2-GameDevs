using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CombatSystem;

public class detectCollisionWaterBreath : MonoBehaviour
{
    private WaterDragonScript enemy;

    /// <summary>
    /// References to all necessary Context
    /// </summary>
    private void Start()
    {
        enemy = GetComponentInParent<WaterDragonScript>();
    }

    /// <summary>
    /// if another Collider is colliding the function is called.
    /// if the other Collider has the Tag Player the Player is getting as much Damage as the Miniboss contains in WaterDamage
    /// </summary>
    /// <param name="other">the colliding Collider</param>
    private void OnParticleCollision(GameObject other)
    {
        Debug.Log(other);
        if(other.tag == "Player")
        {
            combatSystem.LoseHealth(enemy.WaterDamage);
        }
    }
}
