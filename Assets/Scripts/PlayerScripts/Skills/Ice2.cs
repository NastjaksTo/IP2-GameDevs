using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SkillTree;

public class Ice2 : MonoBehaviour
{
    public int damage;
    private GameObject enemy;

    private void Awake()
    {
        damage = 25 * (1 + skillTree.skillLevels[7]);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            enemy = other.gameObject;
            if (enemy.name == "Pandora")
            {
                if (enemy.GetComponent<PandoraAgent>().isInvincible) return;
            }
            enemy.GetComponent<EnemyHealthHandler>().getDamage(damage);
            //anim.SetBool("stunned", true);
            //StartCoroutine(ice2stunned());
        }
    }
   /* 
    IEnumerator ice2stunned() {
        yield return new WaitForSecondsRealtime(5.650f);
        anim.SetBool("stunned", false);
    }*/
}
