using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerAttributes;


public class Fire3 : MonoBehaviour
{
    private int damage;
    private GameObject enemy;

    private void Awake()
    {
        damage = 1000;
        damage += (damage * (int)playerAttributesScript.magicDamage / 100);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            enemy = other.gameObject;
            enemy.GetComponent<EnemyHealthHandler>().getDamage(damage);
        }
    }
}
