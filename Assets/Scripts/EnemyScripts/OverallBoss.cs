using UnityEngine;
using UnityEngine.AI;
using static PlayerSkillsystem;
using static CombatSystem;

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

    private bool isdead;
    private bool phase2;
    private bool doDamage;
    private bool able;

    public bool Phase2 { get => phase2; set => phase2 = value; }
    public PlayerAttributes Player { get => player; set => player = value; }
    public bool Able { get => able; set => able = value; }
    public float Damage { get => damage; set => damage = value; }
    public float ElementalDamage { get => elementalDamage; set => elementalDamage = value; }
    public float Playerlevel { get => playerlevel; set => playerlevel = value; }

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        health = GetComponentInChildren<EnemyHealthHandler>();
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

    private void Update()
    {
        timer += Time.deltaTime;

        playerlevel = playerskillsystem.playerlevel.GetLevel();

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

    private int RandomNumber(int begin, int end)
    {
        timer = 0.0f;
        return Random.Range(begin, end);
    }

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
    public void lookAt()
    {
        if (fov.CanSeePlayer)
        {
            Vector3 relativePos = movePositionTransform.position - transform.position;
            Quaternion rotation = Quaternion.LookRotation(relativePos, Vector3.up);
            this.transform.rotation = rotation;
        }
    }

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

    private void DoDamage()
    {
        if (doDamage)
        {
            if (!phase2)
            {
                combatSystem.LoseHealth(damage);
            }
            if (phase2)
            {
                combatSystem.LoseHealth(damage * 2);
            }
            doDamage = false;
        }
    }

    private void startMagicAttack()
    {
        ps.Play();
    }

    private void stopMagicAttack()
    {
        ps.Stop();
    }

    private void ableToDoDMG()
    {
        able = true;
    }

    private void notAbleToDoDMG()
    {
        able = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            doDamage = true;
        }
    }
}
