using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SkillTree;

public class Ice1 : MonoBehaviour
{
    public Rigidbody rb;
    public int damage;
    private GameObject enemy;

    private void Awake()
    {
        damage = 10 * (1 + skillTree.skillLevels[1]);
    }
    private void OnTriggerEnter(Collider other)
    {
        rb.velocity = Vector3.zero;
        if (other.CompareTag("Enemy"))
        {
            enemy = other.gameObject;
            enemy.GetComponent<EnemyHealthHandler>().getDamage(damage);
            //anim.SetBool("stunned", true);
            //StartCoroutine(ice1stunned());
            Destroy(other.gameObject, 5.25f);
        }
    }
    /*
    IEnumerator ice1stunned() {
        yield return new WaitForSecondsRealtime(5.650f);
        anim.SetBool("stunned", false);
    }*/
}
