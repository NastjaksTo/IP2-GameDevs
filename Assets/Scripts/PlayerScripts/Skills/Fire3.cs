using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerAttributes;


public class Fire3 : MonoBehaviour
{
    private int damage;                 // Integer to save the Damage the spell does.
    private GameObject enemy;           // Reference to the enemy gameobject when colliding.

    /// <summary>
    /// Sets the damage values according to the player skilllevel.
    /// </summary>
    private void Awake()
    {
        damage = 1000;
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
            enemy.GetComponent<EnemyHealthHandler>().getDamage(damage);
        }
    }
}
