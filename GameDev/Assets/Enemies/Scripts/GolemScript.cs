
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GolemScript : MonoBehaviour
{
    private Transform movePositionTransform;
    private PlayerAttributes player;
    private GameObject playerModel;
    private Animator animator;
    private NavMeshAgent navMeshAgent;
    private FoVScript fov;
    private EnemyHealthHandler health;
    private Vector3 spawnpoint;
    private bool doDamage;
    private int attackSwitch;
    private float timer;
    private float timeToChangeAttack;
    private bool idle;
    private float attackRange;
    private bool isdead;


    private int damage;

    /// <summary>
    /// References set to all necessary Context
    /// </summary>
    private void Awake()
    {
        playerModel = GameObject.FindGameObjectWithTag("Player");
        movePositionTransform = playerModel.GetComponent<Transform>();
        player = playerModel.GetComponent<PlayerAttributes>();
        animator = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        fov = GetComponent<FoVScript>();
        health = GetComponentInChildren<EnemyHealthHandler>();
        spawnpoint = this.transform.position;
        attackSwitch = 11;
        timer = 0.0f;
        timeToChangeAttack = 1.5f;
        doDamage = false;
        idle = true;
        attackRange = 8.0f;
        fov.Radius = 50.0f;
        fov.Angle = 120.0f;

        health.Health = 100;
        damage = 20;

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
    /// if the Player is in Range, the Enemy will Run, Shoot or Fly and Shoot towards the Target (Same functionality as Attack()). Once it is in Range it will perform a Meele attack.
    /// 
    /// if the Enemy cant see the Target anymore, it will return to its original Position (Spawnpoint)
    /// </summary>
    private void WalkOrAttack()
    {
        if (fov.CanSeePlayer)
        {
            navMeshAgent.destination = movePositionTransform.position;
            idle = false;
            if (Vector3.Distance(this.transform.position, movePositionTransform.position) < attackRange)
            {
                navMeshAgent.speed = 0;
                navMeshAgent.acceleration = 0;
                Attack();
            }
            else if (Vector3.Distance(this.transform.position, movePositionTransform.position) > attackRange)
            {
                navMeshAgent.speed = 5;
                navMeshAgent.acceleration = 8;
                animator.SetBool("Walk", true);
            }
        }
        if (!fov.CanSeePlayer)
        {
            navMeshAgent.speed = 5;
            navMeshAgent.acceleration = 8;
            navMeshAgent.destination = spawnpoint;
            animator.ResetTrigger("Attack1");
            animator.ResetTrigger("Attack2");
            animator.ResetTrigger("Scream");

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
        animator.SetBool("Idle", true);
        if (timer > timeToChangeAttack)
        {
            changeAttack();
            timer = 0;
            idle = false;
        }


        if (!idle)
        {
            animator.SetBool("Idle", false);

            if (attackSwitch < 5)
            {
                animator.SetTrigger("Attack1");
                idle = true;
            }

            if (attackSwitch >= 5 && attackSwitch <= 10)
            {
                animator.SetTrigger("Attack2");
                idle = true;
            }

            if (attackSwitch > 10)
            {
                animator.SetTrigger("Scream");
                idle = true;
            }
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

                animator.SetTrigger("Get Hit");
                health.Hit = false;
            }


            if (health.Dead && isdead)
            {
                isdead = true;
                animator.SetTrigger("Die");
                navMeshAgent.speed = 0;
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
    /// If the Collider of the Red Boss will Triggercollide with the Player, the bool to deal Damage is set to true;
    /// </summary>
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            doDamage = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            doDamage = false;
        }
    }

    /// <summary>
    /// Every time the timer runs down, a new Random Number between 1 and 12 is picked to choose the next Attackpattern. All Triggers are resetted. 
    /// There is a bigger chance to hit Basic Attack and Tail Attack than Scream.
    /// </summary>
    private void changeAttack()
    {
        attackSwitch = Random.Range(1, 12);
        animator.ResetTrigger("Attack1");
        animator.ResetTrigger("Attack2");
        animator.ResetTrigger("Scream");
    }
}