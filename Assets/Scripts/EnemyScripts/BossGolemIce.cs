using UnityEngine;
using static CombatSystem;
using TMPro;
using UnityEngine.UI;
using static BossArena;

public class BossGolemIce : MonoBehaviour
{
    private OverallBoss boss;
    private FoVScript fov;

    private float damage;
    private float iceDamage;
    
    private EnemyHealthHandler healthHandler;
    private Image healthBar;
    private TextMeshProUGUI textHealthPoints;
    private float maxHealth;
    private int health;

    public float IceDamage { get => iceDamage; set => iceDamage = value; }

    /// <summary>
    /// References set to all necessary Context
    /// </summary>
    private void Awake()
    {
        healthBar = GameObject.Find("IceHealthRepresentation").GetComponent<Image>();
        textHealthPoints = GameObject.Find("IceHealthValue").GetComponent<TextMeshProUGUI>();
        healthHandler = GetComponent<EnemyHealthHandler>();
        boss = GetComponent<OverallBoss>();
        fov = GetComponent<FoVScript>();

        fov.Radius = 100.0f;
        fov.Angle = 180.0f;

        iceDamage = boss.ElementalDamage;
        damage = boss.Damage;
    }
    
    private void Start()
    {
        maxHealth = healthHandler.Health;
    }

    private void CloseArena()
    {
        bossarenaScript.CloseAllArenas();
    }
    
    private void Update()
    {
        if (boss.isdead)
        {
            bossarenaScript.isIceTitanAlive = false;
            Invoke(nameof(CloseArena), 2f);
            bossarenaScript.QuestCompletion();
        }
        healthBar.fillAmount = healthHandler.Health / maxHealth;
        textHealthPoints.text = healthHandler.Health.ToString();
        boss.WalkOrAttack("Walk", "Magic", "BottomSlash", "SlashHit", "Stomp");
        boss.getDamage(5000, "Die");
        boss.lookAt();
    }
}
