
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FatDragonScript : MonoBehaviour
{
    private Transform movePositionTransform;
    private Animator animator;
    private NavMeshAgent navMeshAgent;
    private Vector3 spawnpoint;
    private bool isInRange;
    private bool doDamage;
    private int attackSwitch;
    private int attackSwitchRange;
    private float timer;
    private float timeToChangeAttack;
    private int health;
    private bool idle;
    private float shotSpeed;

    [SerializeField]
    GameObject standProjectileSpawnpoint;

    [SerializeField]
    GameObject flyProjectileSpawnpoint;

    [SerializeField]
    GameObject fireBall;


    private void Awake()
    {
        movePositionTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        animator = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        spawnpoint = this.transform.position;
        isInRange = false;
        attackSwitch = 11;
        attackSwitchRange = 8;
        timer = 0.0f;
        timeToChangeAttack = 2.5f;
        health = 100;
        doDamage = false;
        idle = false;
        shotSpeed = 20.0f;
    }

    private void Update()
    {
        timer += Time.deltaTime;
        WalkOrAttack();
        getDamage();
    }

    private void WalkOrAttack()
    {
        if (Vector3.Distance(movePositionTransform.position, transform.position) <= 50.0f)
        {
            navMeshAgent.destination = movePositionTransform.position;
            navMeshAgent.speed = 5;
            if (Vector3.Distance(this.transform.position, movePositionTransform.position) < 8.5f)
            {
                Attack();
            } else if (Vector3.Distance(this.transform.position, movePositionTransform.position) > 8.5f)
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
                }
            }
        }
        if (Vector3.Distance(movePositionTransform.position, transform.position) > 50.0f)
        {
            navMeshAgent.speed = 5;
            navMeshAgent.destination = spawnpoint;
            animator.ResetTrigger("Basic Attack");
            animator.ResetTrigger("Tail Attack");
            animator.ResetTrigger("Shoot");
            animator.ResetTrigger("Fly and Shoot");
            animator.ResetTrigger("Scream");

            if (Vector3.Distance(this.transform.position, spawnpoint) < 4.0f)
            {

                animator.SetBool("Walk", false);
            }
        }
    }

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

    private void getDamage()
    {
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
            //Make Damage to Player
            Debug.Log("Damage to Player from Crab");
        }
    }

    private void SpawnBulletStand()
    {
        if (movePositionTransform != null)
        {
            GameObject fireball = Instantiate(fireBall, standProjectileSpawnpoint.transform.position, Quaternion.identity);
            Vector3 direction = movePositionTransform.position - standProjectileSpawnpoint.transform.position;

            fireball.GetComponent<Rigidbody>().AddForce(direction.normalized * shotSpeed, ForceMode.Impulse);
        }
    }
    private void SpawnBulletFly()
    {
        if (movePositionTransform != null)
        {
            GameObject fireball = Instantiate(fireBall, flyProjectileSpawnpoint.transform.position, Quaternion.identity);
            Vector3 direction = movePositionTransform.position - flyProjectileSpawnpoint.transform.position;

            fireball.GetComponent<Rigidbody>().AddForce(direction.normalized * shotSpeed, ForceMode.Impulse);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            doDamage = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            isInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            isInRange = false;
        }
    }

    private void changeAttack()
    {
        attackSwitch = Random.Range(1, 13);
        animator.ResetTrigger("Basic Attack");
        animator.ResetTrigger("Tail Attack");
        animator.ResetTrigger("Scream");
    }

    private void changeAttackRange()
    {
        attackSwitchRange = Random.Range(1, 13);
        animator.ResetTrigger("Shoot");
        animator.ResetTrigger("Fly and Shoot");
    }
}
