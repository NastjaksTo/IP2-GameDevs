using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Fire3 : MonoBehaviour
{
    public int damage;
    private GameObject enemy;

    private void Awake()
    {
        damage = 1000;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            enemy = other.gameObject;
            enemy.GetComponent<EnemyHealthHandler>().getDamage(damage);
            Destroy(other.gameObject, 5.55f);
        }
    }
}
