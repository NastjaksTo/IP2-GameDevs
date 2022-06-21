using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;
using static PlayerSkillsystem;

public class PandoraAgent : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform player;

    public LayerMask whatIsGround, whatIsPlayer;

    private EnemyHealthHandler healthHandler;
    public int health;

    public Vector3 walkpoint;
    private bool walkPointSet;
    public float walkPointRange;
    private bool hasPatrollingCooldown;

    public float timeBetweenAttacks;
    private bool alreadyAttacked;

    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;

    public GameObject projectile;
    public Transform spawner;
    public int shootingPower;

    private bool isDead;
    private bool isCurrentlyAttacking;
    
    private Animator anim;

    private void Awake()
    {
        player = GameObject.FindWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        spawner = GameObject.Find("PandoraAttackSpawner").transform;
        anim = GetComponent<Animator>();
        healthHandler = GetComponent<EnemyHealthHandler>();
        healthHandler.Health = health;
    }

    private void Update()
    {
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if(!playerInSightRange && !playerInAttackRange) Patrolling();
        if(playerInSightRange && !playerInAttackRange) ChasePlayer();
        if (playerInAttackRange && playerInSightRange) AttackPlayer();
        GetDamage("hit", "die", 5000);
    }

    private void Patrolling()
    {
        if (isCurrentlyAttacking) return;
        if(isDead) return;
        if (hasPatrollingCooldown) return;
        if (!walkPointSet) SearchWalkPoint();

        if (walkPointSet)
            agent.SetDestination(walkpoint);
        anim.Play("walking");
        Vector3 distanceToWalkPoint = transform.position - walkpoint;

        if (distanceToWalkPoint.magnitude < 1f)
        {
            walkPointSet = false;
            StartCoroutine(patrollingCooldown());
        }
    }
    
    IEnumerator patrollingCooldown()
    {
        hasPatrollingCooldown = true;
        yield return new WaitForSeconds(.66f);
        anim.Play("standingStill");
        yield return new WaitForSecondsRealtime(3f);
        hasPatrollingCooldown = false;
    }

    private void SearchWalkPoint()
    {
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        var position = transform.position;
        walkpoint = new Vector3(position.x + randomX, position.y, position.z + randomZ);

        if (Physics.Raycast(walkpoint, -transform.up, 2f, whatIsGround))
            walkPointSet = true;
    }
    
    private void ChasePlayer()
    {
        if(isCurrentlyAttacking) return;
        if(isDead) return;
        anim.Play("walking");
        agent.SetDestination(player.position);
    }
    
    private void AttackPlayer()
    {
        agent.SetDestination(transform.position);
        
        Vector3 position = new Vector3 (player.position.x, transform.position.y, player.position.z);
        transform.LookAt(position);

        if (!alreadyAttacked && !isDead)
        {
            //ATTACK
            StartCoroutine(currentlyAttacking());
            anim.Play("attackOne");
            if(isDead) return;
            Invoke(nameof(AttackOne), 1.55f);
            
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    IEnumerator currentlyAttacking()
    {
        isCurrentlyAttacking = true;
        yield return new WaitForSecondsRealtime(1.65f);
        isCurrentlyAttacking = false;
    }

    void AttackOne()
    {
        var attackOne = Instantiate(projectile, spawner.position, Quaternion.identity);
        attackOne.GetComponent<Rigidbody>().velocity = (player.position - attackOne.transform.position).normalized * shootingPower;
        Destroy(attackOne, 5f);
    }
    
    private void ResetAttack()
    {
        alreadyAttacked = false;
    }
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
    
    
    //....
    public void GetDamage(string Hit, string Die, int Exp)
    {
        if (healthHandler.Hit)
        {
            if (healthHandler.Health > 0 && alreadyAttacked)
            {
                anim.Play(Hit);
                healthHandler.Hit = false;
            }


            if (healthHandler.Dead && !isDead)
            {
                isDead = true;
                anim.Play(Die);
                agent.speed = 0;
                Destroy(gameObject, 2.0f);
                playerskillsystem.playerlevel.AddExp(Exp);
            }
        }
    }
    //
}
