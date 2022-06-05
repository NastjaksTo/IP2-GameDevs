using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolemTesting : MonoBehaviour
{
    public Transform playerObject;
    public Animator anim;
    public CombatSystem combatsystem;

    public float health = 1000;
    private bool hitting = false;

    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Weapon") && combatsystem._anim.GetCurrentAnimatorStateInfo(0).IsName("lightattack") && !hitting)
        {
            hitting = true;
            health -= 50;
            Debug.Log("AAAAAAAAAAAAAAA");
            Invoke("hitcooldown", 0.85f);
        }
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
