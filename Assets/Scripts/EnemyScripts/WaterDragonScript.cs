using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static PlayerSkillsystem;
using static CombatSystem;

public class WaterDragonScript : MonoBehaviour
{
    private Transform movePositionTransform;
    private PlayerAttributes player;
    private GameObject playerModel;
    private Animator animator;
    private NavMeshAgent navMeshAgent;
    private ParticleSystem ps;
    private FoVScript fov;
    private EnemyHealthHandler health;
    private Vector3 spawnpoint;
    private bool doDamage;
    private int attackSwitch;
    private int attackSwitchRange;
    private float timer;
    private float timeToChangeAttack;
    private bool idle;
    private float attackRange;
    private bool isdead;

    private int damage;
    private int waterDamage;
    private float speed;

    [SerializeField]
    private Collider col;

    public PlayerAttributes Player { get => player; set => player = value; }
    public int WaterDamage { get => waterDamage; set => waterDamage = value; }

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
        ps = GetComponentInChildren<ParticleSystem>();
        health = GetComponentInChildren<EnemyHealthHandler>();
        spawnpoint = this.transform.position;
        attackSwitch = 11;
        attackSwitchRange = 8;
        timer = 0.0f;
        timeToChangeAttack = 1.5f;
        doDamage = false;
        idle = true;
        isdead = false;
        attackRange = navMeshAgent.stoppingDistance;
        speed = navMeshAgent.speed;

        fov.Radius = 50.0f;
        fov.Angle = 120.0f;

        damage = 20 + playerskillsystem.playerlevel.GetLevel() * 3;
        health.Health = 500 + playerskillsystem.playerlevel.GetLevel() * 20;
        waterDamage = 5 + playerskillsystem.playerlevel.GetLevel();
    }
    private void Update()
    {
        timer += Time.deltaTime;
        WalkOrAttack();
        getDamage();

        Vector3 relativePos = movePositionTransform.position - transform.position;
        Quaternion rotation = Quaternion.LookRotation(relativePos, Vector3.up);
        ps.transform.rotation = rotation;
    }

    private void WalkOrAttack()
    {
        if (fov.CanSeePlayer)
        {
            navMeshAgent.destination = movePositionTransform.position;
            navMeshAgent.speed = speed;
            col.isTrigger = false;
            idle = false;
            animator.SetBool("Walk", true);

            if (Vector3.Distance(this.transform.position, movePositionTransform.position) < attackRange)
            {
                Attack();
            }
            else if (Vector3.Distance(this.transform.position, movePositionTransform.position) > attackRange)
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
                    animator.SetTrigger("Water Attack");
                }
                if (attackSwitchRange > 5 && attackSwitchRange <= 10)
                {
                    navMeshAgent.speed = speed;
                    animator.SetBool("Walk", true);
                }
                if (attackSwitchRange > 10)
                {
                    navMeshAgent.speed = speed / 2;
                    animator.SetBool("Walk", false);
                    animator.SetTrigger("Fly and Water");
                    col.isTrigger = true;
                }
            }
        }
        if (!fov.CanSeePlayer)
        {
            navMeshAgent.speed = speed;
            navMeshAgent.destination = spawnpoint;
            animator.ResetTrigger("Scream");
            animator.SetBool("Walk", true);

            if (Vector3.Distance(this.transform.position, spawnpoint) < attackRange)
            {

                animator.SetBool("Walk", false);
            }
        }
    }

    private void Attack()
    {
        navMeshAgent.speed = 0;
        animator.SetBool("Walk", false);
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
                animator.SetTrigger("Basic Attack");
                idle = true;
            }

            if (attackSwitch >= 5 && attackSwitch <= 10)
            {
                animator.SetTrigger("Claw Attack");
                idle = true;
            }

            if (attackSwitch > 10)
            {
                animator.SetTrigger("Scream");
                idle = true;
            }
        }
    }

    private void getDamage()
    {
        if (health.Hit)
        {
            if (health.Health > 0)
            {

                animator.SetTrigger("Get Hit");
                health.Hit = false;
            }

            if (health.Health <= 0 && !isdead)
            {
                isdead = true;
                animator.SetTrigger("Die");
                navMeshAgent.speed = 0;
                Destroy(gameObject, 5.0f);
                playerskillsystem.playerlevel.AddExp(1500);
            }
        }
    }

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


    private void changeAttack()
    {
        attackSwitch = Random.Range(1, 12);
        animator.ResetTrigger("Basic Attack");
        animator.ResetTrigger("Claw Attack");
        animator.ResetTrigger("Scream");
    }

    private void changeAttackRange()
    {
        attackSwitchRange = Random.Range(1, 13);
        animator.ResetTrigger("Water Attack");
        animator.ResetTrigger("Fly and Water");
    }

    private void startSpillWater()
    {
        ps.Play();
    }

    private void stopSpillWater()
    {
        ps.Stop();
    }
}
