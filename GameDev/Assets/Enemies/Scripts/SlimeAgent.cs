using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SlimeAgent : MonoBehaviour
{
    private Transform movePositionTransform;
    private Animator animator;
    private NavMeshAgent navMeshAgent;
    private Vector3 spawnpoint;
    private bool isInRange;
    private float timer;
    private float timeToChangeAttack;
    private int fullHealth;
    private int health;
    private int wichAttack;
    private float attackRange;
    private int ID;
    private bool doDamage;

    [SerializeField]
    GameObject BigSlime;

    private void Awake()
    {
        movePositionTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        animator = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        spawnpoint = this.transform.position;
        isInRange = false;
        timer = 0.0f;
        timeToChangeAttack = 0.8f;
        wichAttack = Random.Range(1, 3);
        fullHealth = 100;
        health = fullHealth;
        attackRange = 2.0f;
        ID = GetInstanceID();
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
        if(doDamage)
        {
            //Make Damage to Player
            Debug.Log("Damage to Player from Slime");
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
        wichAttack = Random.Range(1, 3);
        animator.ResetTrigger("Attack1");
        animator.ResetTrigger("Attack2");
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            doDamage = true;
        }
        if (collision.gameObject.GetComponent<SlimeAgent>())
        {
            if (ID <= collision.gameObject.GetComponent<SlimeAgent>().ID)
            {
                return;
            }
            GameObject O = Instantiate(BigSlime, transform.position, Quaternion.identity) as GameObject;
            Destroy(collision.gameObject);
            Destroy(gameObject);
            O.transform.localScale += new Vector3(0.5f, 0.5f, 0.5f);
            fullHealth = 150;
            health = fullHealth;
        }
    }
}
