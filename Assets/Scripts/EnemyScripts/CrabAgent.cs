using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static PlayerSkillsystem;
using static CombatSystem;

public class CrabAgent : MonoBehaviour
{
    private OverallEnemy enemy;
    private PlayerAttributes player;
    private EnemyHealthHandler health;
    private bool doDamage;

    private float damage;

    /// <summary>
    /// References set to all necessary Context
    /// </summary>
    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerAttributes>();
        health = GetComponentInChildren<EnemyHealthHandler>();
        enemy = GetComponent<OverallEnemy>();

        damage = 15 + enemy.Playerlevel * 3;
        health.Health = 500 + enemy.Playerlevel * 5;
    }

    /// <summary>
    /// timer counting while Update
    /// checking for Target
    /// checking for incoming Damage
    /// </summary>
    private void Update()
    {
        enemy.WalkOrAttack("Run Forward", "Smash Attack", "Stab Attack", 5, 14, 15, "Defend");
        enemy.GetDamage("Take Damage", "Die", 300);
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
