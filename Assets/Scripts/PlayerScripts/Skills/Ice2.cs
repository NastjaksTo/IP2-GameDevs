using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SkillTree;
using static PlayerAttributes;

public class Ice2 : MonoBehaviour
{
    public int damage;
    private GameObject enemy;
    private float stunduration;
    
    private void Awake()
    {
        damage = 25 * (1 + skillTree.skillLevels[7]) + (int)(playerAttributesScript.magicDamage / 4);
        stunduration = 1f + skillTree.skillLevels[7] + playerAttributesScript.magicDamage / 20;
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
            if(enemy.GetComponent<PandoraAgent>() != null)
            {
                enemy.GetComponent<PandoraAgent>().GetStunned(stunduration);
            }
            if(enemy.GetComponent<OverallEnemy>() != null)
            {
                enemy.GetComponent<OverallEnemy>().GetStunned(stunduration);
            }
            if(enemy.GetComponent<OverallBoss>() != null)
            {
                enemy.GetComponent<OverallBoss>().GetStunned(stunduration);
            }
            if (enemy.GetComponent<FatDragonScript>())
            {
                enemy.GetComponent<FatDragonScript>().GetStunned(stunduration);
            }
            if (enemy.GetComponent<GolemScript>())
            {
                enemy.GetComponent<GolemScript>().GetStunned(stunduration);
            }
            if (enemy.GetComponent<WaterDragonScript>())
            {
                enemy.GetComponent<WaterDragonScript>().GetStunned(stunduration);
            }
            enemy.GetComponent<EnemyHealthHandler>().getDamage(damage);
        }
    }
}
