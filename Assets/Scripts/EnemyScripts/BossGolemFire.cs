using UnityEngine;
using static CombatSystem;
using TMPro;
using UnityEngine.UI;
using static BossArena;

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
    
    private EnemyHealthHandler healthHandler;
    private Image healthBar;
    private TextMeshProUGUI textHealthPoints;
    private float maxHealth;
    private int health;
    
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
        healthBar = GameObject.Find("FireHealthRepresentation").GetComponent<Image>();
        textHealthPoints = GameObject.Find("FireHealthValue").GetComponent<TextMeshProUGUI>();
        healthHandler = GetComponent<EnemyHealthHandler>();
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

    /// <summary>
    /// Start is called after Awake
    /// the maxHealth from the Enemyhealthhandler is being read.
    /// </summary>
    private void Start()
    {
        maxHealth = healthHandler.Health;
    }

    /// <summary>
    /// the Bossarena is getting closed
    /// </summary>
    private void CloseArena()
    {
        bossarenaScript.CloseAllArenas();
    }

    /// <summary>
    /// Update is called every frame.
    /// the Healthbar is filled with the numbers of the active Boss.
    /// the necessary functions of the OverallBoss Script are being called.
    /// if the Boss is dead the Quest is completed and the Arena is getting opened.
    /// </summary>
    private void Update()
    {
        if (boss.isdead)
        {
            bossarenaScript.isFireTitanAlive = false;
            Invoke(nameof(CloseArena), 2f);
            bossarenaScript.QuestCompletion();
        }
        healthBar.fillAmount = healthHandler.Health / maxHealth;
        textHealthPoints.text = healthHandler.Health.ToString();
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
