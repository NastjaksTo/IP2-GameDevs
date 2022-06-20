
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static PlayerSkillsystem;
using static CombatSystem;

public class FatDragonScript : MonoBehaviour
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
    private int attackSwitchRange;
    private float timer;
    private float timeToChangeAttack;
    private bool idle;
    private float shotSpeed;
    private float attackRange;
    private bool isdead;

    private int damage;

    [SerializeField]
    GameObject standProjectileSpawnpoint;

    [SerializeField]
    GameObject flyProjectileSpawnpoint;

    [SerializeField]
    GameObject fireBall;

    [SerializeField]
    Collider collider;

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
        attackSwitchRange = 8;
        timer = 0.0f;
        timeToChangeAttack = 1.5f;
        doDamage = false;
        idle = true;
        attackRange = 6.0f;
        shotSpeed = 20.0f;
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
            navMeshAgent.speed = 5;
            collider.isTrigger = false;
            idle = false;
            if (Vector3.Distance(this.transform.position, movePositionTransform.position) < attackRange)
            {
                Attack();
            } else if (Vector3.Distance(this.transform.position, movePositionTransform.position) > attackRange)
            {
                if (timer > timeToChangeAttack)
                {
                    changeAttackRange();
                    timer = 0;
                }

                if (attackSwitchRange < 5)
                {
                    navMeshAgent.speed = 0;
                    animator.SetBool("Walk", false);
                    animator.SetTrigger("Shoot");
                }
                if(attackSwitchRange > 5 && attackSwitchRange <= 10)
                {
                    navMeshAgent.speed = 5;
                    animator.SetBool("Walk", true);
                }
                if (attackSwitchRange > 10)
                {
                    navMeshAgent.speed = 2;
                    animator.SetBool("Walk", false);
                    animator.SetTrigger("Fly and Shoot");
                    collider.isTrigger = true;
                }
            }
        }
        if (!fov.CanSeePlayer)
        {
            navMeshAgent.speed = 5;
            navMeshAgent.destination = spawnpoint;
            animator.ResetTrigger("Basic Attack");
            animator.ResetTrigger("Tail Attack");
            animator.ResetTrigger("Shoot");
            animator.ResetTrigger("Fly and Shoot");
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
        navMeshAgent.speed = 0;
        animator.SetBool("Walk", false);
        animator.SetBool("Idle", true);
        if(timer > timeToChangeAttack)
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
                animator.SetTrigger("Basic Attack");
                idle = true;
            }

            if (attackSwitch >= 5 && attackSwitch <= 10)
            {
                animator.SetTrigger("Tail Attack");
                idle = true;
            }

            if(attackSwitch > 10)
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


            if (health.Dead && !isdead)
            {
                isdead = true;
                animator.SetTrigger("Die");
                navMeshAgent.speed = 0;
                Destroy(gameObject, 5.0f);
                playerskillsystem.playerlevel.AddExp(1500);
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
            combatSystem.LoseHealth(damage);
            doDamage = false;
        }
    }

    /// <summary>
    /// if the Red Boss is shooting a Fireball while Standing the Fireball is spawned in the right Place
    /// </summary>
    private void SpawnBulletStand()
    {
        if (movePositionTransform != null)
        {
            GameObject fireball = Instantiate(fireBall, standProjectileSpawnpoint.transform.position, Quaternion.identity);
            fireball.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
            Vector3 direction = movePositionTransform.position - (standProjectileSpawnpoint.transform.position - new Vector3(0, 1, 0));

            fireball.GetComponent<Rigidbody>().AddForce(direction.normalized * shotSpeed, ForceMode.Impulse);
        }
    }

    /// <summary>
    /// if the Red Boss is shooting a Fireball while Flying the Fireball is spawned in the right Place
    /// </summary>
    private void SpawnBulletFly()
    {
        if (movePositionTransform != null)
        {
            GameObject fireball = Instantiate(fireBall, flyProjectileSpawnpoint.transform.position, Quaternion.identity);
            fireball.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
            Vector3 direction = movePositionTransform.position - (flyProjectileSpawnpoint.transform.position - new Vector3(0, 1, 0));

            fireball.GetComponent<Rigidbody>().AddForce(direction.normalized * shotSpeed, ForceMode.Impulse);
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
        animator.ResetTrigger("Basic Attack");
        animator.ResetTrigger("Tail Attack");
        animator.ResetTrigger("Scream");
    }


    /// <summary>
    /// Every time the timer runs down, a new Random Number between 1 and 12 is picked to choose the next Attackpattern. All Triggers are resetted.
    /// There is a bigger chance to hit no Attack and Shoot than Fly and Shoot.
    /// </summary>
    private void changeAttackRange()
    {
        attackSwitchRange = Random.Range(1, 13);
        animator.ResetTrigger("Shoot");
        animator.ResetTrigger("Fly and Shoot");
    }
}
