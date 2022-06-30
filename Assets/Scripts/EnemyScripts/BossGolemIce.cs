using UnityEngine;
using static CombatSystem;

public class BossGolemIce : MonoBehaviour
{
    private OverallBoss boss;
    private FoVScript fov;

    private float damage;
    private float iceDamage;

    public float IceDamage { get => iceDamage; set => iceDamage = value; }

    /// <summary>
    /// References set to all necessary Context
    /// </summary>
    private void Awake()
    {
        boss = GetComponent<OverallBoss>();
        fov = GetComponent<FoVScript>();

        fov.Radius = 100.0f;
        fov.Angle = 180.0f;

        iceDamage = boss.ElementalDamage;
        damage = boss.Damage;
    }
    private void Update()
    {
        boss.WalkOrAttack("Walk", "Magic", "BottomSlash", "SlashHit", "Stomp");
        boss.getDamage(5000, "Die");
        boss.lookAt();
    }
}
