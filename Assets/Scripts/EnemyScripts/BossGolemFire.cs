using UnityEngine;
using static CombatSystem;

public class BossGolemFire : MonoBehaviour
{
    public static BossGolemFire fireTitan;
    private Transform movePositionTransform;
    private GameObject playerModel;
    private OverallBoss boss;
    private FoVScript fov;

    private bool able;
    private bool doDamage;

    private float damage;
    private float fireDamage;
    private int shotSpeed;


    [SerializeField]
    GameObject projectileSpawnpoint;

    [SerializeField]
    GameObject fireBall;

    public float FireDamage { get => fireDamage; set => fireDamage = value; }

    /// <summary>
    /// References set to all necessary Context
    /// </summary>
    private void Awake()
    {
        fireTitan = this;
        playerModel = GameObject.FindGameObjectWithTag("Player");
        movePositionTransform = playerModel.GetComponent<Transform>();
        boss = GetComponent<OverallBoss>();
        fov = GetComponent<FoVScript>();
        doDamage = false;

        fov.Radius = 100.0f;
        fov.Angle = 180.0f;

        damage = boss.Damage;
        fireDamage = boss.ElementalDamage * 40;
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
