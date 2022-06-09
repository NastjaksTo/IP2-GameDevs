using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossGolemSand : MonoBehaviour
{
    private Transform movePositionTransform;
    private PlayerAttributes player;
    private GameObject playerModel;
    private Animator animator;
    private UnityEngine.AI.NavMeshAgent navMeshAgent;
    private ParticleSystem ps;
    private FoVScript fov;
    private Vector3 spawnpoint;
    private bool doDamage;
    private int attackSwitch;
    private int attackSwitchRange;
    private float timer;
    private float timeToChangeAttack;
    private bool idle;
    private float attackRange;

    private int health;
    private int damage;
    private int earthDamage;

    public PlayerAttributes Player { get => player; set => player = value; }
    public int EarthDamage { get => earthDamage; set => earthDamage = value; }

    /// <summary>
    /// References set to all necessary Context
    /// </summary>
    private void Awake()
    {
        playerModel = GameObject.FindGameObjectWithTag("Player");
        movePositionTransform = playerModel.GetComponent<Transform>();
        player = playerModel.GetComponent<PlayerAttributes>();
        animator = GetComponent<Animator>();
        navMeshAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        fov = GetComponent<FoVScript>();
        ps = GetComponentInChildren<ParticleSystem>();
        spawnpoint = this.transform.position;
        attackSwitch = 11;
        attackSwitchRange = 8;
        timer = 0.0f;
        timeToChangeAttack = 1.5f;
        doDamage = false;
        idle = true;
        attackRange = 8.0f;
        fov.Radius = 100.0f;
        fov.Angle = 180.0f;

        damage = 20;
        earthDamage = 1;
        health = 500;
    }
    private void Update()
    {
        timer += Time.deltaTime;
        WalkOrAttack();
        getDamage();
    }

    private void WalkOrAttack()
    {
        if (fov.CanSeePlayer)
        {
            navMeshAgent.destination = movePositionTransform.position;
            navMeshAgent.speed = 5;
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
                    navMeshAgent.speed = 5;
                    animator.SetBool("Walk", true);
                }
                if (attackSwitchRange > 10)
                {
                    navMeshAgent.speed = 2;
                    animator.SetBool("Walk", false);
                    animator.SetTrigger("Fly and Water");
                }
            }
        }
        if (!fov.CanSeePlayer)
        {
            navMeshAgent.speed = 5;
            navMeshAgent.destination = spawnpoint;
            animator.ResetTrigger("Basic Attack");
            animator.ResetTrigger("Claw Attack");
            animator.ResetTrigger("Water Attack");
            animator.ResetTrigger("Fly and Water");
            animator.ResetTrigger("Scream");

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
        //OnCollisionEnter -- if Player => getDamage
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (health > 0)
            {
                health = health - 20;
                animator.SetTrigger("Get Hit");
            }

            if (health <= 0)
            {
                animator.SetTrigger("Die");
                Destroy(gameObject, 5.0f);
            }
        }
    }

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

    private void startEarthAttack()
    {
        ps.Play();
    }

    private void stopEarthAttack()
    {
        ps.Stop();
    }
}
