using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CrabAgent_Range : MonoBehaviour
{
    private Transform movePositionTransform;
    private Animator animator;
    private NavMeshAgent navMeshAgent;
    private FoVScript fov;
    private Vector3 spawnpoint;
    private int health;
    private GameObject projectileSpawnpoint;
    private float shotSpeed;
    private float fireRate;
    private float damage;

    [SerializeField]
    private float level = 1;

    [SerializeField] 
    GameObject fireball;

    /// <summary>
    /// References set to all necessary Context
    /// </summary>
    private void Awake()
    {
        movePositionTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        projectileSpawnpoint = GetComponentInChildren<Spawnpoint>().gameObject;
        fov = GetComponent<FoVScript>();
        animator = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        spawnpoint = this.transform.position;
        health = 100;
        shotSpeed = 20.0f;
        fireRate = 5.0f;
        damage = level * 10;
        fov.Radius = 25.0f;
        fov.Angle = 180.0f;
    }

    /// <summary>
    /// checking for Target
    /// checking for incoming Damage
    /// </summary>
    private void Update()
    {
        WalkOrAttack();
        getDamage();
    }

    /// <summary>
    /// if the Player is in Range, the Enemy will Jump in Place to show that its triggered. The Range is equal to the Attackrange. The Enemy will attack once the Player is been spotted.
    /// 
    /// if the Enemy cant see the Target anymore, it will stop Jumping and shooting. The Ranged Crab is not able to move.
    /// </summary>
    private void WalkOrAttack()
    {
        if (fov.CanSeePlayer)
        {
            animator.SetTrigger("Jump");
            navMeshAgent.destination = movePositionTransform.position;
            Attack();
        }
        if (!fov.CanSeePlayer)
        {
            navMeshAgent.destination = spawnpoint;

            if (Vector3.Distance(this.transform.position, spawnpoint) < 3.0f)
            {
                animator.SetBool("Walk Backward", false);
            }
        }
    }

    /// <summary>
    /// if the Enemy is nearby the Target the Ranged Enemy is shooting Fireballs in the direction of the Target.
    /// </summary>
    private void Attack()
    {
        fireRate -= Time.deltaTime;
        if(fireRate <= 0)
        {
            animator.SetTrigger("Cast Spell");
            fireRate = 5.0f;
        }
    }

    /// <summary>
    /// if the Ranged Crab is shooting a Fireball it is spawned in the right Place
    /// </summary>
    private void SpawnBullet()
    {
        if(movePositionTransform != null)
            {
            GameObject fireBall = Instantiate(fireball, projectileSpawnpoint.transform.position, Quaternion.identity);
            Vector3 direction = movePositionTransform.position - (projectileSpawnpoint.transform.position - new Vector3(0,1,0));

            fireBall.GetComponent<Rigidbody>().AddForce(direction.normalized * shotSpeed, ForceMode.Impulse);
        } 
    }

    /// <summary>
    /// if the Target is doing Damage to the Enemy, the health is being lowered
    /// if the health is equal or lower 0, the Enemy dies.
    /// </summary>
    private void getDamage()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (health > 0)
            {
                health = health - 20;
                animator.SetTrigger("Take Damage");
            }

            if (health <= 0)
            {
                animator.SetTrigger("Die");
                navMeshAgent.enabled = false;
                Destroy(gameObject, 5.0f);
            }
        }
    }
}
