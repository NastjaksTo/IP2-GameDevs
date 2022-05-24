using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            //make Damage to Player
            Destroy(gameObject);
        }
        Destroy(gameObject, 5);
    }

}
