using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static PlayerSkillsystem;
using static CombatSystem;

public class SlimeAgent : MonoBehaviour
{
    private OverallEnemy enemy;
    private PlayerAttributes player;
    private EnemyHealthHandler health;
    private int fullHealth;
    private int ID;
    private bool doDamage;

    private float damage;

    [SerializeField]
    GameObject BigSlime;

    /// <summary>
    /// References set to all necessary Context
    /// </summary>
    private void Awake()
    {
        enemy = GetComponent<OverallEnemy>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerAttributes>();
        health = GetComponent<EnemyHealthHandler>();

        health.Health = 100 + enemy.Playerlevel * 5;
        fullHealth = (int)health.Health;
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
        enemy.WalkOrAttack("Walk", "Attack1", "Attack2", 5, 15, 0);
        enemy.GetDamage("GetHit", "Die", 100);
    }


    /// <summary>
    /// if the Enemy is able to hit the Player, the Player is getting damaged.
    /// </summary>
    private void DoDamage()
    {
        if(doDamage)
        {
            combatSystem.LoseHealth(damage);
            doDamage = false;
        }
    }

    /// <summary>
    /// If the Trigger Collider of the Slime will collide with the Player, the bool to deal Damage is set to true;
    /// </summary>
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<PlayerAttributes>())
        {
            doDamage = true;
        }
    }

    /// <summary>
    /// if the Slime Collides with another Slime, they merge to a bigger, stronger slime.
    /// </summary>
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<SlimeAgent>())
        {
            if (ID <= collision.gameObject.GetComponent<SlimeAgent>().ID)
            {
                return;
            }
            GameObject O = Instantiate(BigSlime, transform.position, Quaternion.identity) as GameObject;
            Destroy(collision.gameObject);
            Destroy(gameObject);
            O.transform.localScale += new Vector3(0.5f, 0.5f, 0.5f);
            fullHealth = 150;
            health.Health = fullHealth;
        }
    }
}
