using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CombatSystem;

public class detectFireBossMagicCollision : MonoBehaviour
{
    private BossGolemFire enemy;

    private void Start()
    {
        enemy = GetComponentInParent<BossGolemFire>();
    }

    private void OnParticleCollision(GameObject other)
    {
        Debug.Log(other);
        if (other.tag == "Player")
        {
            combatSystem.LoseHealth(enemy.FireDamage);
        }
    }
}
