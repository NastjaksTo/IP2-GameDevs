using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static PlayerSkillsystem;


public class CrabAgent_Range : MonoBehaviour
{
    private OverallEnemy enemy;
    private Transform movePositionTransform;
    private Animator animator;
    private NavMeshAgent navMeshAgent;
    private FoVScript fov;
    private EnemyHealthHandler health;
    private Vector3 spawnpoint;
    private GameObject projectileSpawnpoint;
    private float shotSpeed;
    private float fireRate;
    private float fireBallDamage;

    [SerializeField]
    private float level = 1;

    [SerializeField] 
    GameObject fireball;

    public float FireBallDamage { get => fireBallDamage; set => fireBallDamage = value; }

    /// <summary>
    /// References set to all necessary Context
    /// </summary>
    private void Awake()
    {
        enemy = GetComponent<OverallEnemy>();
        health = GetComponentInChildren<EnemyHealthHandler>();
        fov = GetComponent<FoVScript>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        movePositionTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        projectileSpawnpoint = GetComponentInChildren<Spawnpoint>().gameObject;
        animator = GetComponent<Animator>();

        spawnpoint = this.transform.position;

        fireRate = 5.0f;
        shotSpeed = 20.0f;

        health.Health = 100;
        fireBallDamage = 10;
    }

    /// <summary>
    /// checking for Target
    /// checking for incoming Damage
    /// </summary>
    private void Update()
    {
        enemy.GetDamage("Take Damage", "Die", 300);
        Attack();
        fireRate -= Time.deltaTime;

        lookAt();
    }

    private void lookAt()
    {
        if (fov.CanSeePlayer)
        {
            Vector3 relativePos = movePositionTransform.position - transform.position;
            Quaternion rotation = Quaternion.LookRotation(relativePos, Vector3.up);
            this.transform.rotation = rotation;
        }
    }

    private void Attack()
    {
        if (fov.CanSeePlayer)
        {
            animator.SetBool("Jump", true);
            navMeshAgent.destination = movePositionTransform.position;
            if(fireRate <= 0)
            {
                animator.SetTrigger("Cast Spell");
                fireRate = 5.0f;
            }
        }
        if (!fov.CanSeePlayer)
        {
            navMeshAgent.destination = spawnpoint;
            animator.SetBool("Jump", false);
        }
    }

    private void SpawnBullet()
    {
            if (movePositionTransform != null)
            {
                GameObject fireBall = Instantiate(fireball, projectileSpawnpoint.transform.position, Quaternion.identity);
                Vector3 direction = movePositionTransform.position - (projectileSpawnpoint.transform.position - new Vector3(0, 1, 0));

                fireBall.GetComponent<Rigidbody>().AddForce(direction.normalized * shotSpeed, ForceMode.Impulse);
            }
    }
}
