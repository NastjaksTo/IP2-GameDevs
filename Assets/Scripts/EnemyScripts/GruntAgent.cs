using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static PlayerSkillsystem;
using static CombatSystem;

public class GruntAgent : MonoBehaviour
{
    private OverallEnemy enemy;
    private PlayerAttributes player;
    private EnemyHealthHandler health;
    private bool doDamage;

    private float damage;

    [SerializeField]
    private float level = 1;

    /// <summary>
    /// References set to all necessary Context
    /// </summary>
    private void Awake()
    {
        enemy = GetComponent<OverallEnemy>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerAttributes>();
        health = GetComponentInChildren<EnemyHealthHandler>();

        health.Health = 100;
        damage = level * 10;
    }

    /// <summary>
    /// timer counting while Update
    /// checking for Target
    /// checking for incoming Damage
    /// </summary>
    private void Update()
    {
        enemy.WalkOrAttack("Run", "Attack1", "Attack2", 5, 15, 0);
        enemy.GetDamage("Take Damage", "Die", 200);
    }

    /// <summary>
    /// if the Enemy is able to hit the Player, the Player is getting damaged.
    /// </summary>
    private void DoDamage()
    {
        if (doDamage)
        {
            combatSystem.LoseHealth(damage);
            doDamage = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            doDamage = true;
        }
    }
}
