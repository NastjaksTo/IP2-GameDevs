using UnityEngine;
using static CombatSystem;

public class BossGolemSand : MonoBehaviour
{
    private OverallBoss boss;
    private FoVScript fov;

    private float damage;
    private float earthDamage;

    public float EarthDamage { get => earthDamage; set => earthDamage = value; }

    /// <summary>
    /// References set to all necessary Context
    /// </summary>
    private void Awake()
    {
        boss = GetComponent<OverallBoss>();
        fov = GetComponent<FoVScript>();

        fov.Radius = 100.0f;
        fov.Angle = 180.0f;

        earthDamage = boss.ElementalDamage;
    }
    private void Update()
    {
        boss.WalkOrAttack("Walk", "Magic", "BottomSlash", "SlashHit", "Stomp");
        boss.getDamage(5000, "Die");
        boss.screamAt();
        boss.lookAt();
    }
}
