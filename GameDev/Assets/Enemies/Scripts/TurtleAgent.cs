using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TurtleAgent : MonoBehaviour
{
    private Transform movePositionTransform;
    private PlayerAttributes player;
    private Animator animator;
    private NavMeshAgent navMeshAgent;
    private FoVScript fov;
    private EnemyHealthHandler health;
    private Vector3 spawnpoint;
    private float timer;
    private float timeToChangeAttack;
    private int wichAttack;
    private float attackRange;
    private float endDefend;
    private bool defend;
    private bool doDamage;

    private int damage;


    /// <summary>
    /// References set to all necessary Context
    /// </summary>
    private void Awake()
    {
        movePositionTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerAttributes>();
        animator = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        fov = GetComponent<FoVScript>();
        health = GetComponent<EnemyHealthHandler>();
        spawnpoint = this.transform.position;
        timer = 0.0f;
        timeToChangeAttack = 0.8f;
        wichAttack = Random.Range(1, 4);
        endDefend = 2.0f;
        defend = false;
        attackRange = 2.0f;
        doDamage = false;
        fov.Radius = 6.0f;
        fov.Angle = 100.0f;

        health.Health = 100;
        damage = 10;
    }

    /// <summary>
    /// timer counting while Update
    /// checking for Target
    /// checking for incoming Damage
    /// </summary>
    private void Update()
    {
        timer += Time.deltaTime;
        WalkOrAttack();
        getDamage();
    }

    /// <summary>
    /// if the Player is in Range, the Enemy will Run towards the Target. Once it is in Range it will attack.
    /// 
    /// if the Enemy cant see the Target anymore, it will return to its original Position (Spawnpoint)
    /// </summary>
    private void WalkOrAttack()
    {
        if (fov.CanSeePlayer)
        {
            navMeshAgent.destination = movePositionTransform.position;
            animator.SetBool("isFighting", true);
            animator.SetBool("Walk", true);
            if (Vector3.Distance(this.transform.position, movePositionTransform.position) < attackRange)
            {
                Attack();
            }
        }
        if (!fov.CanSeePlayer)
        {
            navMeshAgent.destination = spawnpoint;
            animator.SetBool("isFighting", false);
            animator.ResetTrigger("Attack1");
            animator.ResetTrigger("Attack2");
            animator.SetBool("Defend", false);

            if (Vector3.Distance(this.transform.position, spawnpoint) < attackRange)
            {
                animator.SetBool("Walk", false);
            }
        }
    }

    /// <summary>
    /// if the Enemy is nearby the Target one of the Three Attackpatterns will be activated and once the Timer is run down there will be a new Random Number to calculate its next move
    /// while Attacking the Enemy ist not Walking
    /// </summary>
    private void Attack()
    {
        animator.SetBool("Walk", false);
        if (wichAttack <= 4)
        {
            animator.SetTrigger("Attack1");
            if (timer > timeToChangeAttack)
            {
                timer = 0;
                changeAttack();
            }
        }

        if (wichAttack > 4)
        {
            animator.SetTrigger("Attack2");
            if (timer > timeToChangeAttack)
            {
                timer = 0;
                changeAttack();
            }
        }

        if (wichAttack == 9)
        {
            defend = true;
            animator.SetBool("Defend", true);
            if (timer > endDefend)
            {
                timer = 0;
                changeAttack();
            }
        }

        if (!defend)
        {
            animator.SetBool("Defend", false);
        }
    }

    /// <summary>
    /// if the Target is doing Damage to the Enemy, the health is being lowered
    /// if the health is equal or lower 0, the Enemy dies.
    /// </summary>
    private void getDamage()
    {
        if (health.Hit)
        {
            if (health.Health > 0)
            {

                animator.SetTrigger("GetHit");
                health.Hit = false;
            }


            if (health.Dead)
            {
                animator.SetTrigger("Die");
                navMeshAgent.enabled = false;
                Destroy(gameObject, 5.0f);
            }
        }
    }

    /// <summary>
    /// if the Enemy is able to hit the Player, the Player is getting damaged.
    /// </summary>
    private void DoDamage()
    {
        if (doDamage)
        {
            player.currentHealth = (int)(player.currentHealth - damage);
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

    /// <summary>
    /// Every time the timer runs down, a new Random Number between 1 and 3 is picked to choose the next Attackpattern. All Triggers are resetted.
    /// </summary>
    private void changeAttack()
    {
        wichAttack = Random.Range(1, 10);
        Debug.Log(wichAttack);
        animator.ResetTrigger("Attack1");
        animator.ResetTrigger("Attack2");
    }
}
