using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SkillTree;

public class Ice3 : MonoBehaviour
{
    private GameObject enemy;
    private float stunduration;

    private void Awake()
    {
        stunduration = 2f;
    }
    
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            enemy = other.gameObject;
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
