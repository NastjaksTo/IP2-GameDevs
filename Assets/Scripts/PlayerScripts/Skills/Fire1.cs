using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SkillTree;

public class Fire1 : MonoBehaviour
{
    public int damage;
    private GameObject enemy;

    private void Awake()
    {
        damage = 50 * (1 + skillTree.skillLevels[0]);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            enemy = other.gameObject;
            enemy.GetComponent<EnemyHealthHandler>().getDamage(damage);
            Debug.Log("hitting" + other.gameObject.name);
        }
    }
}
