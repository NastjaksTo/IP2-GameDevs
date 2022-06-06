using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolemTesting : MonoBehaviour
{
    public Transform playerObject;
    public Animator anim;
    public CombatSystem combatsystem;
    public PlayerAttributes playerattributes;

    public float health = 1000;
    private bool hitting = false;
    public GameObject spell;

    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Weapon") && combatsystem._anim.GetCurrentAnimatorStateInfo(0).IsName("lightattack") && !hitting)
        {
            hitting = true;
            health -= playerattributes.physicalDamage;
            Invoke("hitcooldown", 0.85f);
        }
        if (other.CompareTag("Fire1"))
        {
            spell = other.gameObject;
            var damage = spell.GetComponent<Fire1>().damage;
            health -= damage;
            Destroy(other.gameObject, 0.25f);
        }
        if (other.CompareTag("Fire2"))
        {
            spell = other.gameObject;
            var damage = spell.GetComponent<Fire2>().damage;
            health -= damage;
            Destroy(other.gameObject, 2.55f);
        }
        if (other.CompareTag("Fire3"))
        {
            spell = other.gameObject;
            var damage = spell.GetComponent<Fire3>().damage;
            health -= damage;
            Destroy(other.gameObject, 5.55f);
        }
        if (other.CompareTag("Ice1"))
        {
            spell = other.gameObject;
            var damage = spell.GetComponent<Ice1>().damage;
            health -= damage;
            anim.SetBool("stunned", true);
            StartCoroutine(ice1stunned());
            Destroy(other.gameObject, 5.25f);
        }
        if (other.CompareTag("Ice2"))
        {
            spell = other.gameObject;
            var damage = spell.GetComponent<Ice2>().damage;
            health -= damage;
            anim.SetBool("stunned", true);
            StartCoroutine(ice1stunned());
            Destroy(other.gameObject, 5.25f);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Ice3"))
        {
            spell = other.gameObject;
            var damage = spell.GetComponent<Ice3>().damage;
            health -= damage;
            anim.SetBool("stunned", true);
            StartCoroutine(ice3stunned());
            Destroy(other.gameObject, 15.25f);
        }
    }

    IEnumerator ice1stunned() {
        yield return new WaitForSecondsRealtime(5.650f);
        anim.SetBool("stunned", false);
    }
    IEnumerator ice3stunned() {
        yield return new WaitForSecondsRealtime(15.650f);
        anim.SetBool("stunned", false);
    }
    
    private void hitcooldown()
    {
        hitting = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0)
        {
            anim.SetBool("death", true);
            Destroy(gameObject, 10);
        }
        transform.LookAt(playerObject);
        if (Input.GetKeyDown(KeyCode.U))
        {
            anim.SetBool("golemtesting", true);
        } else anim.SetBool("golemtesting", false);
    }
}
