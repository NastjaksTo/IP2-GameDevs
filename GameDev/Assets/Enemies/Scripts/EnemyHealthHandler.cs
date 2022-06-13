using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthHandler : MonoBehaviour
{
    private int health;
    private bool hit;
    private bool dead;

    public int Health { get => health; set => health = value; }
    public bool Hit { get => hit; set => hit = value; }
    public bool Dead { get => dead; set => dead = value; }

    public void getDamage(int damage)
    {
        health -= damage;
        hit = true;

        if(health <= 0)
        {
            dead = true;
        }
    }
}
