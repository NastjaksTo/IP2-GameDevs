using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossGolemFire : MonoBehaviour
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

    private int damage;
    private int fireDamage;
    private int shotSpeed;
    private bool phase2;


    [SerializeField]
    GameObject projectileSpawnpoint;

    [SerializeField]
    GameObject fireBall;


    public PlayerAttributes Player { get => player; set => player = value; }
    public int FireDamage { get => fireDamage; set => fireDamage = value; }

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
        health = GetComponent<EnemyHealthHandler>();
        spawnpoint = this.transform.position;
        attackSwitch = 11;
        attackSwitchRange = 1;
        timer = 0.0f;
        timeToChangeAttack = 1.5f;
        doDamage = false;
        idle = true;
        attackRange = 10.0f;
        fov.Radius = 100.0f;
        fov.Angle = 180.0f;

        health.Health = 500;
        damage = 20;
        fireDamage = 1;
        shotSpeed = 20;
        phase2 = false;
    }
    private void Update()
    {
        timer += Time.deltaTime;
        WalkOrAttack();
        getDamage();
        DoDamage();
    }

    private void WalkOrAttack()
    {
        if (fov.CanSeePlayer)
        {
            navMeshAgent.destination = movePositionTransform.position;
            navMeshAgent.speed = 5;
            idle = false;
            animator.SetBool("Walk", true);
            //FaceTarget(movePositionTransform.position);

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

                if (attackSwitchRange <= 6)
                {
                    navMeshAgent.speed = 5;
                    animator.SetBool("Walk", true);
                }

                if (attackSwitchRange == 7)
                {
                    navMeshAgent.speed = 5;
                    animator.SetBool("Magic", true);
                }
            }
        }
        if (!fov.CanSeePlayer)
        {
            navMeshAgent.speed = 5;
            navMeshAgent.destination = spawnpoint;

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

            if (attackSwitch < 5)
            {
                animator.SetTrigger("BottomSlash");
                idle = true;
            }

            if (attackSwitch >= 5 && attackSwitch <= 10)
            {
                animator.SetTrigger("SlashHit");
                idle = true;
            }

            if (attackSwitch == 11)
            {
                animator.SetTrigger("Stomp");
                idle = true;
                if (Vector3.Distance(this.transform.position, movePositionTransform.position) < 5.0f)
                {
                    doDamage = true;
                }
            }

            if (attackSwitchRange == 12)
            {
                navMeshAgent.speed = 0;
                animator.SetBool("Walk", false);
                animator.SetTrigger("rangedSlash");
            }
        }
    }

    private void FaceTarget(Vector3 destination)
    {
        Vector3 lookPos = destination - transform.position;
        lookPos.y = 0;
        Quaternion rotation = Quaternion.LookRotation(lookPos);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 5);
    }

    private void getDamage()
    {
        if (health.Hit)
        {
            if (health.Health <= health.Health / 2)
            {
                phase2 = true;
            }
            if (health.Dead)
            {
                animator.SetTrigger("Die");
                navMeshAgent.speed = 0;
                Destroy(gameObject, 5.0f);
            }
        }
    }

    private void DoDamage()
    {
        if (doDamage)
        {
            if (!phase2)
            {
                player.currentHealth = (int)(player.currentHealth - damage);
            }
            if (phase2)
            {
                player.currentHealth = (int)(player.currentHealth - (damage * 2));
            }
            doDamage = false;
        }
    }

    private void spawnBullet()
    {
        if (movePositionTransform != null)
        {
            GameObject fireball = Instantiate(fireBall, projectileSpawnpoint.transform.position, Quaternion.identity);
            fireball.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
            Vector3 direction = movePositionTransform.position - (projectileSpawnpoint.transform.position - new Vector3(0, 5, 0));

            fireball.GetComponent<Rigidbody>().AddForce(direction.normalized * shotSpeed, ForceMode.Impulse);
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
        attackSwitch = Random.Range(1, 13);
    }

    private void changeAttackRange()
    {
        attackSwitchRange = Random.Range(1, 8);
    }

    private void startFireAttack()
    {
        ps.Play();
    }

    private void stopFireAttack()
    {
        ps.Stop();
    }
}
