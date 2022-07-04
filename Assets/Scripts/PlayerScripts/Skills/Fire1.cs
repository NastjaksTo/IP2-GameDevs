using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SkillTree;
using static PlayerAttributes;

public class Fire1 : MonoBehaviour
{
    private int damage;                 // Integer to save the Damage the spell does.
    private GameObject enemy;           // Reference to the enemy gameobject when colliding.

    /// <summary>
    /// Sets the damage values according to the player skilllevel.
    /// </summary>
    private void Awake()
    {
        damage = 50 * (1 + skillTree.skillLevels[0]);
        damage += (damage * (int)playerAttributesScript.magicDamage / 100);
    }

    /// <summary>
    /// Checks if the spell is colliding with an enemy and deals damage to him.
    /// </summary>
    /// <param name="other">Gets the collider of the gameobject this gameobject collides with.</param>
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
            Destroy(gameObject);
        }
        if(other.gameObject.layer == 3 || other.gameObject.layer == 8) Destroy(gameObject);
    }
}
