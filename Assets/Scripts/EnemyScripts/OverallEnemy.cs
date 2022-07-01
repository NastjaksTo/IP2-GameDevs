using System.Collections;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.AI;
using static PlayerSkillsystem;
using static PlayerAttributes;
using static SkillTree;

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
    public bool isStunned;

    private bool isdead;
    private bool defend;
    public float Playerlevel { get => playerlevel; set => playerlevel = value; }

    /// <summary>
    /// References set to all necessary Context
    /// </summary>
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

    /// <summary>
    /// Update is called once every Frame
    /// the Timer to change Attacks is counting
    /// the playerlevel is being checked
    /// the defend bool is continously updated
    /// </summary>
    private void Update()
    {
        timer += Time.deltaTime;
        playerlevel = playerskillsystem.playerlevel.GetLevel();
        health.Defend = defend;
    }

    /// <summary>
    /// returns a random Number between begin (inkl.) and end (exkl.)
    /// </summary>
    /// <param name="begin">the first possible Number</param>
    /// <param name="end">the Number after the last possible Number</param>
    /// <returns></returns>
    public void RandomRange(int begin, int end)
    {
        attackSwitch = Random.Range(begin, end);
    }

    /// <summary>
    /// if the Enemy can see the Player, it is chasing the Player, till the Player cant be seen anymore or the Player is in Attackrange
    /// if the Player cant be seen anymore the Enemy is returning to its Spawnpoint
    /// if the Player is in Attackrange the Enemy is Attacking
    /// </summary>
    /// <param name="Walk">the name of the Parameter used in the Animator</param>
    /// <param name="Attack1">the name of the Parameter used in the Animator</param>
    /// <param name="Attack2">the name of the Parameter used in the Animator</param>
    /// <param name="numAttack1">the name of the Parameter used in the Animator</param>
    /// <param name="numAttack2">the name of the Parameter used in the Animator</param>
    /// <param name="numDefend">the name of the Parameter used in the Animator</param>
    /// <param name="Defend">the name of the Parameter used in the Animator</param>
    public void WalkOrAttack(string Walk, string Attack1, string Attack2, int numAttack1, int numAttack2, int numDefend, [Optional] string Defend)
    {
        if (isStunned) return;
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

    /// <summary>
    /// if the Enemy can see the Player the Bool is necessary to be set.
    /// if the Enemy cant see the Player anymore the Bool is false again.
    /// </summary>
    /// <param name="isFighting">the name of the Parameter used in the Animator</param>
    public void isFighting(string isFighting)
    {
        if (isStunned) return;
        if (fov.CanSeePlayer)
        {
            animator.SetBool(isFighting, true);
        }
        if (!fov.CanSeePlayer)
        {
            animator.SetBool(isFighting, false);
        }
    }

    /// <summary>
    /// if the Player is in Attackrange the Enemy is attacking the Player. Wich Attack it is using is chosen by a Randomnumber.
    /// </summary>
    /// <param name="numAttack1">the name of the Parameter used in the Animator</param>
    /// <param name="numAttack2">the name of the Parameter used in the Animator</param>
    /// <param name="Walk">the name of the Parameter used in the Animator</param>
    /// <param name="Attack1">the name of the Parameter used in the Animator</param>
    /// <param name="Attack2">the name of the Parameter used in the Animator</param>
    /// <param name="numDefend">the name of the Parameter used in the Animator</param>
    /// <param name="Defend">the name of the Parameter used in the Animator</param>
    public void AttackMethod(int numAttack1, int numAttack2, string Walk, string Attack1, string Attack2, int numDefend, string Defend)
    {
        if (isStunned) return;
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

    public void GetStunned(float Duration)
    {
        navMeshAgent.SetDestination(transform.position);
        isStunned = true;
        animator.SetBool("Stunned", true);
        StartCoroutine(Stunned(Duration));
    }
    public IEnumerator Stunned(float time)
    {
        yield return new WaitForSeconds(time);
        animator.SetBool("Stunned", false);
        isStunned = false;
    }

    /// <summary>
    /// if the enemy Health Script is detecting Damage this Function is triggered.
    /// if the Health of the Enemy is greater than 0 the hit Animation is played and the Bool is getting resetted.
    /// if the Health is 0 or below the Boss is dying and transferring experience to the Player
    /// </summary>
    /// <param name="Hit">the name of the Parameter used in the Animator</param>
    /// <param name="Die">the name of the Parameter used in the Animator</param>
    /// <param name="Exp">the Number of the Experience the Player is getting</param>
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
