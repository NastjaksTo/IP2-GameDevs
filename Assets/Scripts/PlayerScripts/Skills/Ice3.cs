using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SkillTree;

public class Ice3 : MonoBehaviour
{
    private GameObject enemy;           // Reference to the enemy gameobject this spells collides with.
    private float stunduration;         // Float to save the stun duration.
    
    /// <summary>
    /// Sets the stunduration according to the player skilllevel.
    /// </summary>
    private void Awake()
    {
        stunduration = 2f;
    }
    
    /// <summary>
    /// Checks if the spell is colliding with an enemy and what type of enemy and stuns them.
    /// </summary>
    /// <param name="other">Gets the collider of the gameobject this gameobject collides with.</param>
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
