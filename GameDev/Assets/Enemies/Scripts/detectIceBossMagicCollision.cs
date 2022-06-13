using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class detectIceBossMagicCollision : MonoBehaviour
{
    private BossGolemIce enemy;

    private void Start()
    {
        enemy = GetComponentInParent<BossGolemIce>();
    }

    private void OnParticleCollision(GameObject other)
    {
        Debug.Log(other);
        if (other.tag == "Player")
        {
            enemy.Player.currentHealth = (int)(enemy.Player.currentHealth - enemy.IceDamage);
        }
    }
}
