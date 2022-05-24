using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TurtleAgent : MonoBehaviour
{
    private Transform movePositionTransform;
    private Animator animator;
    private NavMeshAgent navMeshAgent;
    private Vector3 spawnpoint;
    private bool isInRange;
    private float timer;
    private float timeToChangeAttack;
    private int health;
    private int wichAttack;
    private float attackRange;
    private float endDefend;
    private bool defend;
    private bool doDamage;

    private void Awake()
    {
        movePositionTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        animator = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        spawnpoint = this.transform.position;
        isInRange = false;
        timer = 0.0f;
        timeToChangeAttack = 0.8f;
        wichAttack = Random.Range(1, 4);
        health = 100;
        endDefend = 2.0f;
        defend = false;
        attackRange = 2.0f;
        doDamage = false;
    }

    private void Update()
    {
        timer += Time.deltaTime;
        WalkOrAttack();
        getDamage();
    }

    private void WalkOrAttack()
    {
        if (Vector3.Distance(movePositionTransform.position, transform.position) <= 6.0f)
        {
            navMeshAgent.destination = movePositionTransform.position;
            animator.SetBool("isFighting", true);
            animator.SetBool("Walk", true);
            if (Vector3.Distance(this.transform.position, movePositionTransform.position) < attackRange)
            {
                Attack();
            }
        }
        if (Vector3.Distance(movePositionTransform.position, transform.position) > 6.0f)
        {
            navMeshAgent.destination = spawnpoint;
            animator.SetBool("isFighting", false);
            animator.ResetTrigger("Attack1");
            animator.ResetTrigger("Attack2");
            animator.SetBool("Defend", false);

            if (Vector3.Distance(this.transform.position, spawnpoint) < 2.0f)
            {
                animator.SetBool("Walk", false);
            }
        }
    }

    private void Attack()
    {
        animator.SetBool("Walk", false);
        if (wichAttack == 1)
        {
            animator.SetTrigger("Attack1");
            if (timer > timeToChangeAttack)
            {
                timer = 0;
                changeAttack();
            }
        }

        if (wichAttack == 2)
        {
            animator.SetTrigger("Attack2");
            if (timer > timeToChangeAttack)
            {
                timer = 0;
                changeAttack();
            }
        }

        if (wichAttack == 3)
        {
            defend = true;
            animator.SetBool("Defend", true);
            if (timer > endDefend)
            {
                timer = 0;
                changeAttack();
            }
        }

        if (!defend)
        {
            animator.SetBool("Defend", false);
        }
    }

    private void getDamage()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (health > 0)
            {
                health = health - 20;
                animator.SetTrigger("GetHit");
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
            Debug.Log("Damage to Player from Turtle");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            doDamage = true;
        }
    }

    /*private void OnTriggerEnter(Collider other)
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
    }*/

    private void changeAttack()
    {
        wichAttack = Random.Range(1, 4);
        Debug.Log(wichAttack);
        animator.ResetTrigger("Attack1");
        animator.ResetTrigger("Attack2");
    }
}
