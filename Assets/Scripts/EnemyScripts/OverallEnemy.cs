using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.AI;
using static PlayerSkillsystem;

public class OverallEnemy : MonoBehaviour
{
    private Transform movePositionTransform;
    private Animator animator;
    private NavMeshAgent navMeshAgent;
    private FoVScript fov;
    private EnemyHealthHandler health;

    private Vector3 spawnpoint;

    private int attackSwitch;

    private float timer;
    private float timeToChangeAttack;
    private float endDefend;
    private float attackRange;
    private float speed;
    private float playerlevel;

    private bool isdead;
    private bool defend;
    public float Playerlevel { get => playerlevel; set => playerlevel = value; }
    
    private void Awake()
    {
        movePositionTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        animator = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        fov = GetComponent<FoVScript>();
        health = GetComponentInChildren<EnemyHealthHandler>();

        spawnpoint = this.transform.position;

        timer = 0.0f;
        timeToChangeAttack = 0.8f;
        speed = navMeshAgent.speed;

        endDefend = 2.0f;

        attackRange = navMeshAgent.stoppingDistance;
        attackSwitch = Random.Range(1, 16);

        defend = false;
        isdead = false;
    }

    private void Update()
    {
        timer += Time.deltaTime;
        playerlevel = playerskillsystem.playerlevel.GetLevel();
        health.Defend = defend;
    }

    public void RandomRange(int begin, int end)
    {
        attackSwitch = Random.Range(begin, end);
    }

    public void WalkOrAttack(string Walk, string Attack1, string Attack2, int numAttack1, int numAttack2, int numDefend, [Optional] string Defend)
    {
        if (fov.CanSeePlayer)
        {
            navMeshAgent.destination = movePositionTransform.position;
            animator.SetBool(Walk, true);
            if (Vector3.Distance(this.transform.position, movePositionTransform.position) < attackRange)
            {
                AttackMethod(numAttack1, numAttack2, Walk, Attack1, Attack2, numDefend, Defend);
            }
        }
        if (!fov.CanSeePlayer)
        {
            navMeshAgent.destination = spawnpoint;
            
            if (Vector3.Distance(this.transform.position, spawnpoint) < (attackRange + 1.5f))
            {
                animator.SetBool(Walk, false);
            }
        }
    }

    public void isFighting(string isFighting)
    {
        if (fov.CanSeePlayer)
        {
            animator.SetBool(isFighting, true);
        }
        if (!fov.CanSeePlayer)
        {
            animator.SetBool(isFighting, false);
        }
    }

    public void AttackMethod(int numAttack1, int numAttack2, string Walk, string Attack1, string Attack2, int numDefend, string Defend)
    {
        animator.SetBool(Walk, false);

        if (attackSwitch <= numAttack1)
        {
            animator.SetTrigger(Attack1);
            if (timer > timeToChangeAttack)
            {
                timer = 0;
                RandomRange(1, 16);
            }
        }

        if (attackSwitch <= numAttack2 && attackSwitch > numAttack1)
        {
            animator.SetTrigger(Attack2);
            if (timer > timeToChangeAttack)
            {
                timer = 0;
                RandomRange(1, 16);
            }
        }

        if (attackSwitch == numDefend)
        {
            defend = true;
            animator.SetBool(Defend, true);
            navMeshAgent.speed = 0;
            if (timer > endDefend)
            {
                navMeshAgent.speed = speed;
                timer = 0;
                defend = false;
                RandomRange(1, 16);
            }
        }

        if (!defend)
        {
            animator.SetBool(Defend, false);
        }
    }

    public void GetDamage(string Hit, string Die, int Exp)
    {
        if (health.Hit)
        {
            if (health.Health > 0)
            {
                animator.SetTrigger(Hit);
                health.Hit = false;
            }

            if (health.Health <= 0 && !isdead)
            {
                isdead = true;
                animator.SetTrigger(Die);
                navMeshAgent.speed = 0;
                Destroy(gameObject, 5.0f);
                playerskillsystem.playerlevel.AddExp(Exp);
            }
        }
    }
}
