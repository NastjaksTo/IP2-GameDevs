using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static PlayerSkillsystem;
using static CombatSystem;

public class TurtleAgent : MonoBehaviour
{
    private PlayerAttributes player;
    private EnemyHealthHandler health;
    private OverallEnemy enemy;
    private bool doDamage;

    private float damage;


    /// <summary>
    /// References set to all necessary Context
    /// </summary>
    private void Awake()
    {
        enemy = GetComponent<OverallEnemy>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerAttributes>();
        health = GetComponent<EnemyHealthHandler>();

        health.Health = 100 + enemy.Playerlevel * 5;
        damage = 10 + enemy.Playerlevel * 2;
    }

    /// <summary>
    /// timer counting while Update
    /// checking for Target
    /// checking for incoming Damage
    /// </summary>
    private void Update()
    {
        enemy.isFighting("isFighting");
        enemy.WalkOrAttack("Walk", "Attack1", "Attack2", 5, 14, 15, "Defend");
        enemy.GetDamage("GetHit", "Die", 100);
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

    /// <summary>
    /// If the Trigger Collider of the Turtle will collide with the Player, the bool to deal Damage is set to true;
    /// </summary>
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            doDamage = true;
        }
    }
}
