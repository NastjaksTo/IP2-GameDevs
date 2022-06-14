using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class detectCollisionWaterBreath : MonoBehaviour
{
    private WaterDragonScript enemy;

    private void Start()
    {
        enemy = GetComponentInParent<WaterDragonScript>();
    }

    private void OnParticleCollision(GameObject other)
    {
        Debug.Log(other);
        if(other.tag == "Player")
        {
            enemy.Player.currentHealth = (int)(enemy.Player.currentHealth - enemy.WaterDamage);
        }
    }
}
