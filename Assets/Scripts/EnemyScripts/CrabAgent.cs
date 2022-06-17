using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static PlayerSkillsystem;

public class CrabAgent : MonoBehaviour
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
    private bool defend;
    private float timer;
    private float timeToChangeAttack;
    private float endDefend;
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
        health = GetComponentInChildren<EnemyHealthHandler>();
        fov = GetComponent<FoVScript>();
        spawnpoint = this.transform.position;
        attackOrRoll = Random.Range(1, 4);
        defend = false;
        timer = 0.0f;
        timeToChangeAttack = 0.8f;
        endDefend = 2.0f;
        doDamage = false;
        fov.Radius = 15.0f;
        fov.Angle = 180.0f;

        damage = level * 10;
        health.Health = 100;
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
            animator.SetBool("Run Forward", true);
            if(Vector3.Distance(this.transform.position, movePositionTransform.position) < 4.0f)
            {
                Attack();
            }
        }
        if (!fov.CanSeePlayer)
        {
            navMeshAgent.destination = spawnpoint;
            animator.ResetTrigger("Smash Attack");
            animator.ResetTrigger("Stab Attack");
            animator.SetBool("Defend", false);

            if (Vector3.Distance(this.transform.position, spawnpoint) < 4.0f)
            {

                animator.SetBool("Run Forward", false);
            }
        }
    }

    /// <summary>
    /// if the Enemy is nearby the Target one of the Three Attackpatterns will be activated and once the Timer is run down there will be a new Random Number to calculate its next move
    /// while Attacking the Enemy ist not Running
    /// </summary>
    private void Attack()
    {
        if (attackOrRoll == 1)
        {
            animator.SetBool("Run Forward", false);
            animator.SetTrigger("Smash Attack");
            if(timer > timeToChangeAttack)
            {
                timer = 0;
                changeAttack();
            }
        }

        if (attackOrRoll == 2)
        {
            animator.SetBool("Run Forward", false);
            animator.SetTrigger("Stab Attack");
            if (timer > timeToChangeAttack)
            {
                timer = 0;
                changeAttack();
            }
        }

        if (attackOrRoll == 3)
        {
            defend = true;
            animator.SetBool("Run Forward", false);
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

                if (defend)
                {
                    health.Health += 5;
                }
                animator.SetTrigger("Take Damage");
                health.Hit = false;
            }


            if (health.Dead && !isdead)
            {
                isdead = true;
                animator.SetTrigger("Die");
                navMeshAgent.speed = 0;
                Destroy(gameObject, 5.0f);
                playerskillsystem.playerlevel.AddExp(300);
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
        attackOrRoll = Random.Range(1, 4);
        defend = false;
        animator.ResetTrigger("Stab Attack");
        animator.ResetTrigger("Smash Attack");
    }
}
