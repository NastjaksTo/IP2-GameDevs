using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
            player.currentHealth = player.currentHealth - 20;
            Destroy(gameObject);
        }
        Destroy(gameObject, 5);
    }

}
