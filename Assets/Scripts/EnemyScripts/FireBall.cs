using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CombatSystem;

public class FireBall : MonoBehaviour
{
    private PlayerAttributes player;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerAttributes>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            combatSystem.LoseHealth(25);
            Destroy(gameObject);
        }
        Destroy(gameObject, 5);
    }

}
