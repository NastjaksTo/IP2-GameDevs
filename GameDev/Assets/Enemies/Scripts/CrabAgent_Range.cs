using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CrabAgent_Range : MonoBehaviour
{
    private Transform movePositionTransform;
    private Animator animator;
    private NavMeshAgent navMeshAgent;
    private Vector3 spawnpoint;
    private bool isInRange;
    private int health;
    private GameObject projectileSpawnpoint;
    private float shotSpeed;
    private float fireRate;
    private float damage;

    [SerializeField]
    private float level = 1;

    [SerializeField] 
    GameObject fireball;

    private void Awake()
    {
        movePositionTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        projectileSpawnpoint = GetComponentInChildren<Spawnpoint>().gameObject;
        animator = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        spawnpoint = this.transform.position;
        isInRange = false;
        health = 100;
        shotSpeed = 20.0f;
        fireRate = 5.0f;
        damage = level * 10;
    }

    private void Update()
    {
        WalkOrAttack();
        getDamage();
    }

    private void WalkOrAttack()
    {
        if (Vector3.Distance(movePositionTransform.position, transform.position) <= 25.0f)
        {
            animator.SetTrigger("Jump");
            navMeshAgent.destination = movePositionTransform.position;
            Attack();
        }
        if (Vector3.Distance(movePositionTransform.position, transform.position) > 25.0f)
        {
            navMeshAgent.destination = spawnpoint;

            if (Vector3.Distance(this.transform.position, spawnpoint) < 3.0f)
            {
                animator.SetBool("Walk Backward", false);
            }
        }
    }

    private void Attack()
    {
        fireRate -= Time.deltaTime;
        if(fireRate <= 0)
        {
            animator.SetTrigger("Cast Spell");
            fireRate = 5.0f;
        }
    }

    private void SpawnBullet()
    {
        if(movePositionTransform != null)
            {
            GameObject fireBall = Instantiate(fireball, projectileSpawnpoint.transform.position, Quaternion.identity);
            Vector3 direction = movePositionTransform.position - projectileSpawnpoint.transform.position;

            fireBall.GetComponent<Rigidbody>().AddForce(direction.normalized * shotSpeed, ForceMode.Impulse);
        } 
    }

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
                Destroy(gameObject, 5.0f);
            }
        }
    }
}
