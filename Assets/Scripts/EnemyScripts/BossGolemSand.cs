using System;
using UnityEngine;
using static CombatSystem;
using TMPro;
using UnityEngine.UI;
using static BossArena;

public class BossGolemSand : MonoBehaviour
{
    private OverallBoss boss;
    private FoVScript fov;

    private float damage;
    private float earthDamage;

    private EnemyHealthHandler healthHandler;
    private Image healthBar;
    private TextMeshProUGUI textHealthPoints;
    private float maxHealth;
    private int health;
    
    public float EarthDamage { get => earthDamage; set => earthDamage = value; }

    /// <summary>
    /// References set to all necessary Context
    /// </summary>
    private void Awake()
    {
        healthBar = GameObject.Find("EarthHealthRepresentation").GetComponent<Image>();
        textHealthPoints = GameObject.Find("EarthHealthValue").GetComponent<TextMeshProUGUI>();
        healthHandler = GetComponent<EnemyHealthHandler>();
        boss = GetComponent<OverallBoss>();
        fov = GetComponent<FoVScript>();
        fov.Radius = 100.0f;
        fov.Angle = 180.0f;

        earthDamage = boss.ElementalDamage;
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
            bossarenaScript.isEarthTitanAlive = false;
            Invoke(nameof(CloseArena), 2f);
            bossarenaScript.QuestCompletion();
        }
        healthBar.fillAmount = healthHandler.Health / maxHealth;
        textHealthPoints.text = healthHandler.Health.ToString();
        boss.WalkOrAttack("Walk", "Magic", "BottomSlash", "SlashHit", "Stomp");
        boss.getDamage(10000, "Die");
        boss.screamAt();
        boss.lookAt();
    }
}
