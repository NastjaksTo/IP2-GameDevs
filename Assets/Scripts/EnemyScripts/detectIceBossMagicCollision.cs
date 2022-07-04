using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CombatSystem;

public class detectIceBossMagicCollision : MonoBehaviour
{
    private BossGolemIce enemy;

    /// <summary>
    /// References to all necessary Context
    /// </summary>
    private void Start()
    {
        enemy = GetComponentInParent<BossGolemIce>();
    }

    /// <summary>
    /// if another Collider is colliding the function is called.
    /// if the other Collider has the Tag Player the Player is getting as much Damage as the Boss contains in ElementalDamage
    /// </summary>
    /// <param name="other">the colliding Collider</param>
    private void OnParticleCollision(GameObject other)
    {
        Debug.Log(other);
        if (other.tag == "Player")
        {
            combatSystem.LoseHealth(enemy.IceDamage);
        }
    }
}
