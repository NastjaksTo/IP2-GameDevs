using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ice3 : MonoBehaviour
{
    public int damage;
    private GameObject enemy;

    private void Awake()
    {
        damage = 1;
    }
    
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            enemy = other.gameObject;
            enemy.GetComponent<EnemyHealthHandler>().getDamage(damage);
            //anim.SetBool("stunned", true);
            //StartCoroutine(ice3stunned());
            Destroy(other.gameObject, 15.25f);
        }
    }
/*
    IEnumerator ice3stunned() {
        yield return new WaitForSecondsRealtime(15.650f);
        anim.SetBool("stunned", false);
    }
*/
}
