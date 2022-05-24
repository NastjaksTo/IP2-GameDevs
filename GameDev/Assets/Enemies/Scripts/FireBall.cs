using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (gameObject.tag == "Player")
        {
            Debug.Log("Damage to Player from Fireball");
            //make Damage to Player
            Destroy(gameObject);
        }
        Destroy(gameObject, 5);
    }

}
