using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CombatSystem;

public class detectIceBossMagicCollision : MonoBehaviour
{
    private BossGolemIce enemy;

    private void Start()
    {
        enemy = GetComponentInParent<BossGolemIce>();
    }

    private void OnParticleCollision(GameObject other)
    {
        Debug.Log(other);
        if (other.tag == "Player")
        {
            combatSystem.LoseHealth(enemy.IceDamage);
        }
    }
}
