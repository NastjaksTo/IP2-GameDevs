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
        if(other.gameObject.layer == 3 || other.gameObject.layer == 8) rb.velocity = Vector3.zero;
        if (other.CompareTag("Enemy"))
        {
            rb.velocity = Vector3.zero;
            enemy = other.gameObject;
            if (enemy.name == "Pandora")
            {
                if (enemy.GetComponent<PandoraAgent>().isInvincible) return;
            }
            enemy.GetComponent<EnemyHealthHandler>().getDamage(damage);
            //anim.SetBool("stunned", true);
            //StartCoroutine(ice1stunned());
        }
    }
    /*
    IEnumerator ice1stunned() {
        yield return new WaitForSecondsRealtime(5.650f);
        anim.SetBool("stunned", false);
    }*/
}
