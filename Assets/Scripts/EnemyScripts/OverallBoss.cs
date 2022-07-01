using UnityEngine;
using UnityEngine.AI;
using static PlayerSkillsystem;
using static CombatSystem;
using static EnemySoundHandler;

public class OverallBoss : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;
    private EnemyHealthHandler health;
    private Animator animator;
    private ParticleSystem ps;
    private FoVScript fov;
    private GameObject playerModel;
    private Transform movePositionTransform;
    private PlayerAttributes player;
    private Vector3 spawnpoint;

    private float attackRange;
    private float timer;
    private float timeToChangeAttack;
    private float speed;
    private float magicDirection;
    private float damage;
    private float elementalDamage;
    private float playerlevel;

    private int switchRange;
    private int switchMelee;

    public bool isdead;
    private bool phase2;
    private bool doDamage;
    private bool able;

    public bool Phase2 { get => phase2; set => phase2 = value; }
    public PlayerAttributes Player { get => player; set => player = value; }
    public bool Able { get => able; set => able = value; }
    public float Damage { get => damage; set => damage = value; }
    public float ElementalDamage { get => elementalDamage; set => elementalDamage = value; }
    public float Playerlevel { get => playerlevel; set => playerlevel = value; }

    /// <summary>
    /// References set to all necessary Context
    /// </summary>
    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        health = GetComponent<EnemyHealthHandler>();
        animator = GetComponent<Animator>();
        ps = GetComponentInChildren<ParticleSystem>();
        player = GetComponent<PlayerAttributes>();
        fov = GetComponent<FoVScript>();
        playerModel = GameObject.FindGameObjectWithTag("Player");
        movePositionTransform = playerModel.GetComponent<Transform>();
        spawnpoint = this.transform.position;
        isdead = false;

        attackRange = navMeshAgent.stoppingDistance;
        timeToChangeAttack = 2f;
        timer = 0.0f;
        speed = navMeshAgent.speed;

        doDamage = false;
        magicDirection = 0.5f;

        health.Health = 600 + playerlevel * 100;
        damage = 30 + playerlevel * 5;
        elementalDamage = 4 + playerlevel / 4;
    }

    /// <summary>
    /// Update is called once every Frame
    /// the Timer to change Attacks is counting
    /// the playerlevel is being checked
    /// if the Enemy is able to do Damage the funktion is getting called
    /// its getting checked in wich phase the Boss is currently
    /// </summary>
    private void Update()
    {
        timer += Time.deltaTime;

        playerlevel = playerskillsystem.playerlevel.GetLevel();

        Debug.Log(health.Health);

        if (able)
        {
            DoDamage();
        }

        if (!phase2)
        {
            timeToChangeAttack = 2.0f;
        }
        if (phase2)
        {
            timeToChangeAttack = 1.0f;
        }
    }

    /// <summary>
    /// returns a random Number between begin (inkl.) and end (exkl.)
    /// </summary>
    /// <param name="begin">the first possible Number</param>
    /// <param name="end">the Number after the last possible Number</param>
    /// <returns></returns>
    private int RandomNumber(int begin, int end)
    {
        timer = 0.0f;
        return Random.Range(begin, end);
    }

    /// <summary>
    /// if the Enemy can see the Player, it is chasing (sometimes casting some Ranged Attacks) the Player, till the Player cant be seen anymore or the Player is in Attackrange
    /// if the Player cant be seen anymore the Enemy is returning to its Spawnpoint
    /// if the Player is in Attackrange the Enemy is Attacking
    /// </summary>
    /// <param name="Walk">the name of the Parameter used in the Animator</param>
    /// <param name="Magic">the name of the Parameter used in the Animator</param>
    /// <param name="Attack1">the name of the Parameter used in the Animator</param>
    /// <param name="Attack2">the name of the Parameter used in the Animator</param>
    /// <param name="Stomp">the name of the Parameter used in the Animator</param>
    public void WalkOrAttack(string Walk, string Magic, string Attack1, string Attack2, string Stomp)
    {
        if (fov.CanSeePlayer)
        {
            navMeshAgent.destination = movePositionTransform.position;
            navMeshAgent.speed = speed;
            animator.SetBool(Walk, true);

            if (Vector3.Distance(this.transform.position, movePositionTransform.position) < attackRange)
            {
                Attack(Walk, Attack1, Attack2, Stomp, Magic);
            }
            else if (Vector3.Distance(this.transform.position, movePositionTransform.position) > attackRange)
            {
                if (timer > timeToChangeAttack)
                {
                    switchRange = RandomNumber(1, 8);
                }

                if (switchRange <= 5)
                {
                    navMeshAgent.speed = speed;
                    animator.SetBool(Walk, true);
                }

                if (switchRange > 6)
                {
                    navMeshAgent.speed = 0;
                    animator.SetBool(Magic, true);
                }
            }
        }
        if (!fov.CanSeePlayer)
        {
            navMeshAgent.speed = speed;
            navMeshAgent.destination = spawnpoint;

            if (Vector3.Distance(this.transform.position, spawnpoint) < attackRange)
            {

                animator.SetBool(Walk, false);
            }
        }
    }

    /// <summary>
    /// if the Player is in Attackrange the Enemy is attacking the Player. Wich Attack it is using is chosen by a Randomnumber.
    /// </summary>
    /// <param name="Walk">the name of the Parameter used in the Animator</param>
    /// <param name="Attack1">the name of the Parameter used in the Animator</param>
    /// <param name="Attack2">the name of the Parameter used in the Animator</param>
    /// <param name="Stomp">the name of the Parameter used in the Animator</param>
    /// <param name="Magic">the name of the Parameter used in the Animator</param>
    private void Attack(string Walk, string Attack1, string Attack2, string Stomp, string Magic)
    {
        navMeshAgent.speed = 0;
        animator.SetBool(Walk, false);

        if (timer > timeToChangeAttack)
        {
            switchMelee = RandomNumber(1, 7);

            if (switchMelee <= 2)
            {
                animator.SetTrigger(Attack1);
            }

            if (switchMelee > 3 && switchMelee <= 4)
            {
                animator.SetTrigger(Attack2);
            }

            if (switchMelee == 5)
            {
                animator.SetTrigger(Stomp);
                if (Vector3.Distance(this.transform.position, movePositionTransform.position) < attackRange)
                {
                    doDamage = true;
                    DoDamage();
                }
            }

            if (switchMelee == 6)
            {
                navMeshAgent.speed = 0;
                animator.SetBool(Walk, false);
                animator.SetTrigger(Magic);
            }
        }
    }

    /// <summary>
    /// if the enemy Health Script is detecting Damage this Function is triggered.
    /// if the Health of the Enemy is below 50% the Boss is thrown into phase 2.
    /// if the Health is 0 or below the Boss is dying and transferring experience to the Player
    /// </summary>
    /// <param name="Exp">the Number of the Experience the Player is getting</param>
    /// <param name="Die">the name of the Parameter used in the Animator</param>
    public void getDamage(int Exp, string Die)
    {
        if (health.Hit)
        {
            if (health.Health <= health.Health / 2)
            {
                phase2 = true;
            }
            if (health.Health <= 0 && !isdead)
            {
                isdead = true;
                animator.SetTrigger(Die);
                navMeshAgent.speed = 0;
                playerskillsystem.playerlevel.AddExp(Exp);
                Destroy(gameObject, 5.0f);
            }
        }
    }

    /// <summary>
    /// if the Enemy is seeing the Player, the Enemy is always looking always straight to the Player
    /// </summary>
    public void lookAt()
    {
        if (fov.CanSeePlayer)
        {
            Vector3 relativePos = movePositionTransform.position - transform.position;
            Quaternion rotation = Quaternion.LookRotation(relativePos, Vector3.up);
            this.transform.rotation = rotation;
        }
    }

    /// <summary>
    /// this function rotates the ParticleSystem in a 60 degree angle infront of the boss from left to the right
    /// </summary>
    public void screamAt()
    {
        if (ps.transform.eulerAngles.y >= 210)
        {
            magicDirection = -0.5f;
        }
        if (ps.transform.eulerAngles.y <= 150)
        {
            magicDirection = 0.5f;
        }
        ps.transform.Rotate(0, magicDirection, 0 * Time.deltaTime, Space.World);
    }

    /// <summary>
    /// if the Enemy is doing Damage the damage that the Player is loosing is transferred.
    /// Although the hit Sound is played.
    /// in the 2 Phase the Enemy is doing 30% more damage.
    /// </summary>
    private void DoDamage()
    {
        if (doDamage)
        {
            if (!phase2)
            {
                enemySoundhandler.hitSound();
                combatSystem.LoseHealth(damage);
            }
            if (phase2)
            {
                enemySoundhandler.hitSound();
                combatSystem.LoseHealth(damage * 1.3f);
            }
            doDamage = false;
        }
    }

    /// <summary>
    /// starting the ParticleSystem
    /// </summary>
    private void startMagicAttack()
    {
        ps.Play();
    }

    /// <summary>
    /// stopping the ParticleSystem
    /// </summary>
    private void stopMagicAttack()
    {
        ps.Stop();
    }

    /// <summary>
    /// is called while the Animation is playing
    /// </summary>
    private void ableToDoDMG()
    {
        able = true;
    }

    /// <summary>
    /// is called while the Animation is playing
    /// </summary>
    private void notAbleToDoDMG()
    {
        able = false;
    }

    /// <summary>
    /// is called when another Collider is triggering the own Collider
    /// Only fully runned if the other Collider has the Player Tag
    /// </summary>
    /// <param name="other">the colliding Collider</param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            doDamage = true;
        }
    }
}
