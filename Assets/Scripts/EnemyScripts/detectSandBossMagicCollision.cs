using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CombatSystem;

public class detectSandBossMagicCollision : MonoBehaviour
{
    private BossGolemSand enemy;

    private void Start()
    {
        enemy = GetComponentInParent<BossGolemSand>();
    }

    private void OnParticleCollision(GameObject other)
    {
        Debug.Log(other);
        if (other.tag == "Player")
        {
            combatSystem.LoseHealth(enemy.EarthDamage);
        }
    }
}
