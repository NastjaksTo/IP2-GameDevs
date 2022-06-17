using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static PlayerSkillsystem;

public class GruntAgent : MonoBehaviour
{
    private PlayerAttributes player;
    private Transform movePositionTransform;
    private Animator animator;
    private NavMeshAgent navMeshAgent;
    private FoVScript fov;
    private EnemyHealthHandler health;
    private Vector3 spawnpoint;
    private bool doDamage;
    private int attackOrRoll;
    private float timer;
    private float timeToChangeAttack;
    private int attackRange;
    private bool isdead;

    private float damage;

    [SerializeField]
    private float level = 1;

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
        health = GetComponentInChildren<EnemyHealthHandler>();
        spawnpoint = this.transform.position;
        attackOrRoll = Random.Range(1, 3);
        timer = 0.0f;
        timeToChangeAttack = 0.8f;
        doDamage = false;
        fov.Radius = 15.0f;
        fov.Angle = 180.0f;

        health.Health = 100;
        damage = level * 10;
        attackRange = 4;
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
            animator.SetBool("Run", true);
            animator.SetBool("Walk", false);
            if (Vector3.Distance(this.transform.position, movePositionTransform.position) < attackRange)
            {
                Attack();
            }
        }
        if (!fov.CanSeePlayer)
        {
            navMeshAgent.destination = spawnpoint;
            animator.SetBool("Walk", true);
            animator.ResetTrigger("Attack1");
            animator.ResetTrigger("Attack2");

            if (Vector3.Distance(this.transform.position, spawnpoint) < attackRange)
            {
                animator.SetBool("Walk", false);
                animator.SetBool("Run", false);
            }
        }
    }

    /// <summary>
    /// if the Enemy is nearby the Target one of the Three Attackpatterns will be activated and once the Timer is run down there will be a new Random Number to calculate its next move
    /// while Attacking the Enemy ist not Running
    /// </summary>
    private void Attack()
    {
        animator.SetBool("Run", false);
        if (attackOrRoll == 1)
        {
            animator.SetTrigger("Attack1");
            if (timer > timeToChangeAttack)
            {
                timer = 0;
                changeAttack();
            }
        }

        if (attackOrRoll == 2)
        {
            animator.SetTrigger("Attack2");
            if (timer > timeToChangeAttack)
            {
                timer = 0;
                changeAttack();
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

                animator.SetTrigger("Take Damage");
                health.Hit = false;
            }


            if (health.Dead && !isdead)
            {
                isdead = true;
                animator.SetTrigger("Die");
                navMeshAgent.speed = 0;
                Destroy(gameObject, 5.0f);
                playerskillsystem.playerlevel.AddExp(200);
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            doDamage = true;
        }
    }

    /// <summary>
    /// Every time the timer runs down, a new Random Number between 1 and 3 is picked to choose the next Attackpattern. All Triggers are resetted.
    /// </summary>
    private void changeAttack()
    {
        attackOrRoll = Random.Range(1, 3);
        animator.ResetTrigger("Attack1");
        animator.ResetTrigger("Attack2");
    }
}
