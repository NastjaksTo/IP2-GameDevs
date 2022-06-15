using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class detectSandBossMagicCollision : MonoBehaviour
{
    private BossGolemSand enemy;

    private void Start()
    {
        enemy = GetComponentInParent<BossGolemSand>();
    }

    private void OnParticleCollision(GameObject other)
    {
        Debug.Log(other);
        if (other.tag == "Player")
        {
            enemy.Player.currentHealth = (int)(enemy.Player.currentHealth - enemy.EarthDamage);
        }
    }
}
