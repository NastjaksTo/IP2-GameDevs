using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthHandler : MonoBehaviour
{
    private float health;
    private bool hit;
    private bool dead;
    private bool defend;

    public bool Hit { get => hit; set => hit = value; }
    public bool Dead { get => dead; set => dead = value; }
    public float Health { get => health; set => health = value; }
    public bool Defend { get => defend; set => defend = value; }

    public void getDamage(int damage)
    {
        if (!defend)
        {
            health -= damage;
            hit = true;
        }
        if (defend)
        {
            health -= damage / 2;
            hit = true;
        }

        if(health <= 0)
        {
            dead = true;
        }
    }
}
