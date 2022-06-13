using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SlimeAgent : MonoBehaviour
{
    private Transform movePositionTransform;
    private PlayerAttributes player;
    private Animator animator;
    private NavMeshAgent navMeshAgent;
    private FoVScript fov;
    private EnemyHealthHandler health;
    private Vector3 spawnpoint;
    private float timer;
    private float timeToChangeAttack;
    private int fullHealth;
    private int wichAttack;
    private float attackRange;
    private int ID;
    private bool doDamage;

    private int damage;
    private bool isdead;

    [SerializeField]
    GameObject BigSlime;

    /// <summary>
    /// References set to all necessary Context
    /// </summary>
    private void Awake()
    {
        movePositionTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerAttributes>();
        animator = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        fov = GetComponent<FoVScript>();
        health = GetComponent<EnemyHealthHandler>();
        spawnpoint = this.transform.position;
        timer = 0.0f;
        timeToChangeAttack = 0.8f;
        wichAttack = Random.Range(1, 3);
        attackRange = 2.0f;
        ID = GetInstanceID();
        doDamage = false;
        fov.Radius = 6.0f;
        fov.Angle = 100.0f;

        health.Health = 100;
        fullHealth = health.Health;
        damage = 10;

    }

    /// <summary>
    /// timer counting while Update
    /// checking for Target
    /// checking for incoming Damage
    /// </summary>
    private void Update()
    {
        timer += Time.deltaTime;
        WalkOrAttack();
        getDamage();
    }

    /// <summary>
    /// if the Player is in Range, the Enemy will Walk towards the Target. Once it is in Range it will attack.
    /// 
    /// if the Enemy cant see the Target anymore, it will return to its original Position (Spawnpoint)
    /// </summary>
    private void WalkOrAttack()
    {
        if (fov.CanSeePlayer)
        {
            navMeshAgent.destination = movePositionTransform.position;
            animator.SetBool("isFighting", true);
            animator.SetBool("Walk", true);
            if (Vector3.Distance(this.transform.position, movePositionTransform.position) < attackRange)
            {
                Attack();
            }
        }
        if (!fov.CanSeePlayer)
        {
            navMeshAgent.destination = spawnpoint;
            animator.SetBool("isFighting", false);
            animator.ResetTrigger("Attack1");
            animator.ResetTrigger("Attack2");

            if (Vector3.Distance(this.transform.position, spawnpoint) < attackRange)
            {
                animator.SetBool("Walk", false);
            }
        }
    }

    /// <summary>
    /// if the Enemy is nearby the Target one of the Two Attackpatterns will be activated and once the Timer is run down there will be a new Random Number to calculate its next move
    /// while Attacking the Enemy ist not Walking
    /// </summary>
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

    /// <summary>
    /// if the Target is doing Damage to the Enemy, the health is being lowered
    /// if the health is equal or lower 0, the Enemy dies.
    /// </summary>
    private void getDamage()
    {
        if (health.Hit)
        {
            if (health.Health > 0)
            {
                
                    animator.SetTrigger("GetHit");
                    health.Hit = false;
               
            }

            if (health.Dead && !isdead)
            {
                isdead = true;
                animator.SetTrigger("Die");
                navMeshAgent.speed = 0;
                Destroy(gameObject, 5.0f);
            }
        }
    }

    /// <summary>
    /// if the Enemy is able to hit the Player, the Player is getting damaged.
    /// </summary>
    private void DoDamage()
    {
        if(doDamage)
        {
            player.currentHealth = (int)(player.currentHealth - damage);
            doDamage = false;
        }
    }

    /// <summary>
    /// If the Trigger Collider of the Slime will collide with the Player, the bool to deal Damage is set to true;
    /// </summary>
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<PlayerAttributes>())
        {
            doDamage = true;
        }
    }

    /// <summary>
    /// Every time the timer runs down, a new Random Number between 1 and 2 is picked to choose the next Attackpattern. All Triggers are resetted.
    /// </summary>
    private void changeAttack()
    {
        wichAttack = Random.Range(1, 3);
        animator.ResetTrigger("Attack1");
        animator.ResetTrigger("Attack2");
    }

    /// <summary>
    /// if the Slime Collides with another Slime, they merge to a bigger, stronger slime.
    /// </summary>
    private void OnCollisionEnter(Collision collision)
    {
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
            health.Health = fullHealth;
        }
    }
}
