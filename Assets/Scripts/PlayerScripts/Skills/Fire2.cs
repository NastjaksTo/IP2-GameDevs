using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SkillTree;

public class Fire2 : MonoBehaviour
{
    public int damage;
    private GameObject enemy;

    private void Awake()
    {
        damage = 100 * (1 + skillTree.skillLevels[6]);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            enemy = other.gameObject;
            if (enemy.name == "Pandora")
            {
                if (enemy.GetComponent<PandoraAgent>().isInvincible) return;
            }
            enemy.GetComponent<EnemyHealthHandler>().getDamage(damage);
        }
    }
}
