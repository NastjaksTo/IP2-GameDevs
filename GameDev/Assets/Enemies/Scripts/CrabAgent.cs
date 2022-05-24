using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CrabAgent : MonoBehaviour
{
    private Transform movePositionTransform;
    private Animator animator;
    private NavMeshAgent navMeshAgent;
    private Vector3 spawnpoint;
    private bool isInRange;
    private bool doDamage;
    private int attackOrRoll;
    private bool defend;
    private float timer;
    private float timeToChangeAttack;
    private float endDefend;
    private int health;

    private void Awake()
    {
        movePositionTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        animator = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        spawnpoint = this.transform.position;
        isInRange = false;
        attackOrRoll = Random.Range(1, 4);
        defend = false;
        timer = 0.0f;
        timeToChangeAttack = 0.8f;
        endDefend = 2.0f;
        health = 100;
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
        if (Vector3.Distance(movePositionTransform.position, transform.position) <= 15.0f)
        {
            navMeshAgent.destination = movePositionTransform.position;
            animator.SetBool("Run Forward", true);
            if(Vector3.Distance(this.transform.position, movePositionTransform.position) < 5.0f)
            {
                Attack();
            }
        }
        if (Vector3.Distance(movePositionTransform.position, transform.position) > 15.0f)
        {
            navMeshAgent.destination = spawnpoint;
            animator.ResetTrigger("Smash Attack");
            animator.ResetTrigger("Stab Attack");
            animator.SetBool("Defend", false);

            if (Vector3.Distance(this.transform.position, spawnpoint) < 4.0f)
            {

                animator.SetBool("Run Forward", false);
            }
        }
    }

    private void Attack()
    {
        if (attackOrRoll == 1)
        {
            animator.SetBool("Run Forward", false);
            animator.SetTrigger("Smash Attack");
            if(timer > timeToChangeAttack)
            {
                timer = 0;
                changeAttack();
            }
        }

        if (attackOrRoll == 2)
        {
            animator.SetBool("Run Forward", false);
            animator.SetTrigger("Stab Attack");
            if (timer > timeToChangeAttack)
            {
                timer = 0;
                changeAttack();
            }
        }

        if (attackOrRoll == 3)
        {
            defend = true;
            animator.SetBool("Run Forward", false);
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
                animator.SetTrigger("Take Damage");
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

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            doDamage = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            isInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Player")
        {
            isInRange = false;
        }
    }

    private void changeAttack()
    {
        attackOrRoll = Random.Range(1, 4);
        defend = false;
        animator.ResetTrigger("Stab Attack");
        animator.ResetTrigger("Smash Attack");
    }
}
