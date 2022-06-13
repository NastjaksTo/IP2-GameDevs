using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthHandler : MonoBehaviour
{
    private int health;

    public int Health { get => health; set => health = value; }

    public void getDamage(int damage)
    {
        health -= damage;
    }
}
