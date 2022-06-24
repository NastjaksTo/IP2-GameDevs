using UnityEngine;
using static CombatSystem;
using static PlayerSkillsystem;

public class BossGolemSand : MonoBehaviour
{
    private OverallBoss boss;
    private FoVScript fov;
    private EnemyHealthHandler health;
    private bool doDamage;

    private int damage;
    private int earthDamage;

    public int EarthDamage { get => earthDamage; set => earthDamage = value; }

    /// <summary>
    /// References set to all necessary Context
    /// </summary>
    private void Awake()
    {
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

        earthDamage = 1;
    }
    private void Update()
    {
        boss.WalkOrAttack("Walk", "Magic", "BottomSlash", "SlashHit", "Stomp");
        boss.getDamage(5000, "Die");
        boss.screamAt();
        boss.lookAt();

        doDamage = boss.DoDamage;

        if (boss.Able)
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
}
