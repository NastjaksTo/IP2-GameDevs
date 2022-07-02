using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SkillTree;
using static PlayerAttributes;

public class Ice1 : MonoBehaviour
{
    public Rigidbody rb;
    private GameObject enemy;
    private float stunduration;

    private void Awake()
    {
        stunduration = 1f + skillTree.skillLevels[1]/2f + playerAttributesScript.magicDamage / 20f;
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == 3 || other.gameObject.layer == 8) rb.velocity = Vector3.zero;
        if (other.CompareTag("Enemy"))
        {
            rb.velocity = Vector3.zero;
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
        }
    }
}
