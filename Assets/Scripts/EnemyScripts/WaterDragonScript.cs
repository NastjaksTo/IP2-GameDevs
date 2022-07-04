using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static PlayerSkillsystem;
using static CombatSystem;

public class WaterDragonScript : MonoBehaviour
{
    private Transform movePositionTransform;
    private PlayerAttributes player;
    private GameObject playerModel;
    private Animator animator;
    private NavMeshAgent navMeshAgent;
    private ParticleSystem ps;
    private FoVScript fov;
    private EnemyHealthHandler health;
    private Vector3 spawnpoint;
    private bool doDamage;
    private int attackSwitch;
    private int attackSwitchRange;
    private float timer;
    private float timeToChangeAttack;
    private bool idle;
    private float attackRange;
    private bool isdead;
    private bool isStunned;

    private int damage;
    private int waterDamage;
    private float speed;

    [SerializeField]
    private Collider col;

    public PlayerAttributes Player { get => player; set => player = value; }
    public int WaterDamage { get => waterDamage; set => waterDamage = value; }

    /// <summary>
    /// References set to all necessary Context
    /// </summary>
    private void Awake()
    {
        playerModel = GameObject.FindGameObjectWithTag("Player");
        movePositionTransform = playerModel.GetComponent<Transform>();
        player = playerModel.GetComponent<PlayerAttributes>();
        animator = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        fov = GetComponent<FoVScript>();
        ps = GetComponentInChildren<ParticleSystem>();
        health = GetComponentInChildren<EnemyHealthHandler>();
        spawnpoint = this.transform.position;
        attackSwitch = 11;
        attackSwitchRange = 8;
        timer = 0.0f;
        timeToChangeAttack = 1.5f;
        doDamage = false;
        idle = true;
        isdead = false;
        attackRange = navMeshAgent.stoppingDistance;
        speed = navMeshAgent.speed;

        fov.Radius = 50.0f;
        fov.Angle = 120.0f;

        
    }

    private void Start()
    {
        damage = 5 + playerskillsystem.playerlevel.GetLevel() * 3;
        health.Health = 500 + playerskillsystem.playerlevel.GetLevel() * 20;
        waterDamage = 5 + playerskillsystem.playerlevel.GetLevel();
    }

    /// <summary>
    /// timer for Attackchange counting while Update
    /// checking for Target
    /// checking for incoming Damage
    /// rotating the ParticleSystem in direction of the Player
    /// </summary>
    private void Update()
    {
        timer += Time.deltaTime;
        WalkOrAttack();
        getDamage();

        Vector3 relativePos = movePositionTransform.position - transform.position;
        Quaternion rotation = Quaternion.LookRotation(relativePos, Vector3.up);
        ps.transform.rotation = rotation;
    }

    /// <summary>
    /// if the Player can be seen, the Enemy will Run towards the Target and perform some Rangeattacks. Once it is in Attackrange it will perform a Meele attack.
    /// 
    /// if the Enemy cant see the Target anymore, it will return to its original Position (Spawnpoint)
    /// </summary>
    private void WalkOrAttack()
    {
        if (isStunned) return;
        if (fov.CanSeePlayer)
        {
            navMeshAgent.destination = movePositionTransform.position;
            navMeshAgent.speed = speed;
            col.isTrigger = false;
            idle = false;
            animator.SetBool("Walk", true);

            if (Vector3.Distance(this.transform.position, movePositionTransform.position) < attackRange)
            {
                Attack();
            }
            else if (Vector3.Distance(this.transform.position, movePositionTransform.position) > attackRange)
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
                    animator.SetTrigger("Water Attack");
                }
                if (attackSwitchRange > 5 && attackSwitchRange <= 10)
                {
                    navMeshAgent.speed = speed;
                    animator.SetBool("Walk", true);
                }
                if (attackSwitchRange > 10)
                {
                    navMeshAgent.speed = speed / 2;
                    animator.SetBool("Walk", false);
                    animator.SetTrigger("Fly and Water");
                    col.isTrigger = true;
                }
            }
        }
        if (!fov.CanSeePlayer)
        {
            navMeshAgent.speed = speed;
            navMeshAgent.destination = spawnpoint;
            animator.ResetTrigger("Scream");
            animator.SetBool("Walk", true);

            if (Vector3.Distance(this.transform.position, spawnpoint) < attackRange)
            {

                animator.SetBool("Walk", false);
            }
        }
    }

    /// <summary>
    /// if the Enemy is nearby the Target one of the Three Attackpatterns will be activated and once the Timer is run down there will be a new Random Number to calculate its next move.
    /// While Attacking the Enemy ist not Walking
    /// </summary>
    private void Attack()
    {
        if (isStunned) return;
        navMeshAgent.speed = 0;
        animator.SetBool("Walk", false);
        if (timer > timeToChangeAttack)
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
                animator.SetTrigger("Claw Attack");
                idle = true;
            }

            if (attackSwitch > 10)
            {
                animator.SetTrigger("Scream");
                idle = true;
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

                animator.SetTrigger("Get Hit");
                health.Hit = false;
            }

            if (health.Health <= 0 && !isdead)
            {
                isdead = true;
                animator.SetTrigger("Die");
                navMeshAgent.speed = 0;
                Destroy(gameObject, 5.0f);
                playerskillsystem.playerlevel.AddExp(3000);
            }
        }
    }

    /// <summary>
    /// Stuns the enemy, making him do nothing for a set amount of time.
    /// </summary>
    /// <param name="Duration">Duration of the stun.</param>
    public void GetStunned(float Duration)
    {
        navMeshAgent.SetDestination(transform.position);
        isStunned = true;
        animator.SetBool("Stunned", true);
        StartCoroutine(Stunned(Duration));
    }
    
    /// <summary>
    /// Starts the duration of the stun.
    /// </summary>
    /// <param name="time">Duration of the stun.</param>
    /// <returns></returns>
    public IEnumerator Stunned(float time)
    {
        yield return new WaitForSeconds(time);
        animator.SetBool("Stunned", false);
        isStunned = false;
    }

    /// <summary>
    /// if the Enemy is able to hit the Player, the Player is getting damaged.
    /// </summary>
    private void DoDamage()
    {
        if (doDamage)
        {
            combatSystem.LoseHealth(damage);
            doDamage = false;
        }
    }

    /// <summary>
    /// if the Collider is getting triggered by the Player the Enemy is able to do Damage
    /// </summary>
    /// <param name="other">the Players Hitbox</param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            doDamage = true;
        }
    }

    /// <summary>
    /// if the Collider exiting trigger state the Enemy is no longer able to deal Damage
    /// </summary>
    /// <param name="other">the Players Hitbox</param>
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            doDamage = false;
        }
    }

    /// <summary>
    /// Every time the timer runs down, a new Random Number between 1 and 11 is picked to choose the next Attackpattern. All Triggers are resetted. 
    /// There is a bigger chance to hit Basic Attack and Claw Attack than Scream.
    /// </summary>
    private void changeAttack()
    {
        attackSwitch = Random.Range(1, 12);
        animator.ResetTrigger("Basic Attack");
        animator.ResetTrigger("Claw Attack");
        animator.ResetTrigger("Scream");
    }

    /// <summary>
    /// Every time the timer runs down, a new Random Number between 1 and 12 is picked to choose the next Attackpattern. All Triggers are resetted. 
    /// There is a bigger chance to hit Walk than Water Attack or Fly and Water.
    /// </summary>
    private void changeAttackRange()
    {
        attackSwitchRange = Random.Range(1, 13);
        animator.ResetTrigger("Water Attack");
        animator.ResetTrigger("Fly and Water");
    }

    /// <summary>
    /// Start ParticleSystem
    /// </summary>
    private void startSpillWater()
    {
        ps.Play();
    }

    /// <summary>
    /// Stop ParticleSystem
    /// </summary>
    private void stopSpillWater()
    {
        ps.Stop();
    }
}
