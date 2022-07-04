using System;
using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using static PlayerSkillsystem;
using Random = UnityEngine.Random;

public class SkeletonAgent : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform player;

    public LayerMask whatIsGround, whatIsPlayer;

    private OverallEnemy enemy;
    private EnemyHealthHandler healthHandler;
    public float health;

    private FoVScript fov;

    public GameObject projectile;
    public Transform spawner;

    public Vector3 walkpoint;
    private bool walkPointSet;
    public float walkPointRange;
    private bool hasPatrollingCooldown;

    public float timeBetweenAttacks = 2;
    private bool alreadyAttacked;

    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;
    
    private bool isDead;
    private bool isCurrentlyAttacking;
    public bool isAttacking;
    private bool inAnimation;

    public int sworddamage;

    private bool casting;

    private bool hasSpellCooldown;
    
    private Animator anim;
    private ThirdPersonController controller;

    public string skeletonName;
    private WayPoints waypoints;
    private Transform currentWaypoint;

    /// <summary>
    /// Gets all references and assigns the values of skeleton enemies.
    /// </summary>
    private void Awake()
    {
        player = GameObject.FindWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        enemy = GetComponent<OverallEnemy>();
        fov = GetComponent<FoVScript>();
        healthHandler = GetComponent<EnemyHealthHandler>();
        healthHandler.Health = (int)health;
        waypoints = GameObject.Find("WayPoints"+skeletonName).GetComponent<WayPoints>();
        currentWaypoint = waypoints.GetNextWaypoint(currentWaypoint);
        transform.position = currentWaypoint.position;
    }

    /// <summary>
    /// Creates spheres around skeletons which check for certain objects inside them.
    /// If an player is inside the sightrange, the skeleton starts to chase him.
    /// If an Player is inside the attackrange, the skeleton starts to attack him.
    /// </summary>
    private void Update()
    {
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position+transform.up*1.25f, attackRange, whatIsPlayer);
        if((!fov.CanSeePlayer || !playerInSightRange) && !playerInAttackRange) Patrolling();
        if((fov.CanSeePlayer || playerInSightRange) && !playerInAttackRange && !inAnimation) ChasePlayer();
        if(playerInAttackRange && (fov.CanSeePlayer || playerInSightRange)) AttackPlayer();
        GetDamage("Hit", "Die", 1000);
    }

    /// <summary>
    /// Starts patrolling to the next Waypoint.
    /// </summary>
    private void Patrolling()
    {
        if (enemy.isStunned) return;
        if (isCurrentlyAttacking) return;
        if(isDead) return;
        if (hasPatrollingCooldown) return;
        if (Vector3.Distance(transform.position, currentWaypoint.position) < 1.25f)
        {
            currentWaypoint = waypoints.GetNextWaypoint(currentWaypoint);
        }
        agent.SetDestination(currentWaypoint.position);
        Vector3 position = new Vector3 (currentWaypoint.position.x, transform.position.y, currentWaypoint.position.z);
        transform.LookAt(position);
        anim.SetBool("walking", true);
        agent.speed = 3;
        anim.SetBool("chasing", false);
        Vector3 distanceToWalkPoint = transform.position - currentWaypoint.position;

        if (distanceToWalkPoint.magnitude < 1.3f)
        {
            StartCoroutine(patrollingCooldown());
        }
    }
    
    /// <summary>
    /// Starts the cooldown of patrolling. That way the skeleton stops at each waypoint for a set amount of time.
    /// </summary>
    /// <returns></returns>
    IEnumerator patrollingCooldown()
    {
        hasPatrollingCooldown = true;
        yield return new WaitForSeconds(.66f);
        anim.SetBool("walking", false);
        yield return new WaitForSeconds(5f);
        hasPatrollingCooldown = false;
    }
    
    /// <summary>
    /// Starts the cooldown of spell usage.
    /// </summary>
    /// <returns></returns>
    IEnumerator spellCooldown()
    {
        hasSpellCooldown = true;
        yield return new WaitForSeconds(5f);
        hasSpellCooldown = false;
    }

    /// <summary>
    /// Chases the player.
    /// </summary>
    private void ChasePlayer()
    {
        if (enemy.isStunned) return;
        if(isDead) return;
        anim.SetBool("walking", false);
        agent.speed = 5f;
        anim.SetBool("chasing", true);
        agent.SetDestination(player.position);
        Vector3 position = new Vector3 (player.position.x, transform.position.y, player.position.z);
        transform.LookAt(position);
        if(!hasSpellCooldown) CastSpell();
    }
    
    /// <summary>
    /// Casts a spell.
    /// </summary>
    private void CastSpell()
    {
        if (enemy.isStunned) return;
        anim.SetBool("chasing", false);
        anim.SetBool("walking", false);
        agent.SetDestination(transform.position);
        
        Vector3 position = new Vector3 (player.position.x, transform.position.y, player.position.z);
        transform.LookAt(position);

        if (!alreadyAttacked && !isDead && !inAnimation)
        {
            anim.SetBool("spellcast", true);
        }
    }

    /// <summary>
    /// Animation Event, which gets called when the skeleton starts casting.
    /// Instantiates the spell.
    /// </summary>
    public void Casting()
    {
        StartCoroutine(spellCooldown());
        var attackOne = Instantiate(projectile, spawner.position, Quaternion.identity);
        attackOne.GetComponent<Rigidbody>().velocity = (player.position - attackOne.transform.position).normalized;
        casting = false;
        anim.SetBool("spellcast", false);
    }
    
    /// <summary>
    /// Starts attacking the player.
    /// </summary>
    private void AttackPlayer()
    {
        if (enemy.isStunned) return;
        if (alreadyAttacked) return;
        anim.SetBool("walking", false);
        anim.SetBool("chasing", false);
        agent.SetDestination(transform.position);
        
        Vector3 position = new Vector3 (player.position.x, transform.position.y, player.position.z);
        transform.LookAt(position);

        if (!alreadyAttacked && !isDead && !inAnimation)
        {
            anim.SetBool("lightattack", true);
        }
        Invoke(nameof(ResetAttack), timeBetweenAttacks);
    }
    
    /// <summary>
    /// Resets the bool alreadyAttacked to false. That way the skeleton can attack again.
    /// </summary>
    private void ResetAttack()
    {
        alreadyAttacked = false;
    }
    
    /// <summary>
    /// Animation Event, which gets called when the skeleton hits the player.
    /// Sets the bool of isAttacking to true.
    /// </summary>
    public void LightAttack(AnimationEvent animationEvent)
    {
        isAttacking = true;
        Invoke(nameof(StopAttack), 0.025f);
    }

    /// <summary>
    /// Animation Event, which gets called when the skeleton starts attacking.
    /// Sets the inAnimation bool to true.
    /// </summary>
    public void StartAttack(AnimationEvent animationEvent)
    {
        inAnimation = true;
    }
    
    /// <summary>
    /// Resets all attacking values.
    /// </summary>
    public void StopAttack()
    {
        isAttacking = false;
        inAnimation = false;
        anim.SetBool("lightattack", false);
    }
    
    /// <summary>
    /// Gets called when the skeleton is getting hit.
    /// Checks if the skeleton should die.
    /// Calls die animation and gives player the experience.
    /// Destroys the skeleton after 2 seconds.
    /// </summary>
    /// <param name="Hit">Name of the hit animation.</param>
    /// <param name="Die">Name of the die animation.</param>
    /// <param name="Exp">Amount of experience.</param>
    public void GetDamage(string Hit, string Die, int Exp)
    {
        if (healthHandler.Hit)
        {
            if (healthHandler.Health > 0)
            {
                healthHandler.Hit = false;
            }
            if (healthHandler.Health <= 0 && !isDead)
            {
                Debug.Log(health);
                Debug.Log(healthHandler.Health);
                isDead = true;
                anim.SetTrigger(Die);
                agent.speed = 0;
                playerskillsystem.playerlevel.AddExp(Exp);
                Destroy(gameObject, 2.0f);
            }
        }
    }
    
    /// <summary>
    /// Draws gizmos for each sphere (attackrange, sightrange);
    /// </summary>
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position+transform.up*1.25f, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
}
