using UnityEngine;
using static CombatSystem;
using static PlayerSkillsystem;

public class BossGolemFire : MonoBehaviour
{
    private Transform movePositionTransform;
    private GameObject playerModel;
    private OverallBoss boss;
    private FoVScript fov;
    private EnemyHealthHandler health;

    private bool able;
    private bool doDamage;

    private int damage;
    private int fireDamage;
    private int shotSpeed;


    [SerializeField]
    GameObject projectileSpawnpoint;

    [SerializeField]
    GameObject fireBall;

    public int FireDamage { get => fireDamage; set => fireDamage = value; }

    /// <summary>
    /// References set to all necessary Context
    /// </summary>
    private void Awake()
    {
        playerModel = GameObject.FindGameObjectWithTag("Player");
        movePositionTransform = playerModel.GetComponent<Transform>();
        boss = GetComponent<OverallBoss>();
        fov = GetComponent<FoVScript>();
        health = GetComponent<EnemyHealthHandler>();
        doDamage = false;
        fov.Radius = 100.0f;
        fov.Angle = 180.0f;

        health.Health = 500;
        damage = 20;

        fov.Radius = 100.0f;
        fov.Angle = 180.0f;

        fireDamage = 1;
    }
    private void Update()
    {
        boss.WalkOrAttack("Walk", "Magic", "BottomSlash", "SlashHit", "Stomp");
        boss.getDamage(5000, "Die");
        boss.screamAt();
        boss.lookAt();

        doDamage = boss.DoDamage;

        if (able)
        {
            DoDamage();
        }
    }

    private void DoDamage()
    {
        if (doDamage)
        {
            if (!boss.Phase2)
            {
                combatSystem.LoseHealth(damage);
            }
            if (boss.Phase2)
            {
                combatSystem.LoseHealth(damage * 2);
            }
            boss.DoDamage = false;
        }
    }

    private void spawnBullet()
    {
        if (movePositionTransform != null)
        {
            GameObject fireball = Instantiate(fireBall, projectileSpawnpoint.transform.position, Quaternion.identity);
            fireball.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
            Vector3 direction = movePositionTransform.position - (projectileSpawnpoint.transform.position - new Vector3(0, 5, 0));

            fireball.GetComponent<Rigidbody>().AddForce(direction.normalized * shotSpeed, ForceMode.Impulse);
        }
    }
}
