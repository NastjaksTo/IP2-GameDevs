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

    private float damage;
    private float fireDamage;
    private float shotSpeed;


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
        shotSpeed = 20.0f;

        fov.Radius = 100.0f;
        fov.Angle = 180.0f;

        fireDamage = boss.ElementalDamage * 40;
    }
    private void Update()
    {
        boss.WalkOrAttack("Walk", "Magic", "BottomSlash", "SlashHit", "Stomp");
        boss.getDamage(5000, "Die");
        boss.lookAt();
    }


    private void spawnBullet()
    {
        if (movePositionTransform != null)
        {
            GameObject fireball = Instantiate(fireBall, projectileSpawnpoint.transform.position, Quaternion.identity);
            Vector3 direction = movePositionTransform.position - projectileSpawnpoint.transform.position;

            fireball.GetComponent<Rigidbody>().AddForce(direction.normalized * shotSpeed, ForceMode.Impulse);
        }
    }
}
