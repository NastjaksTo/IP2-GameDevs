using System;
using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using UnityEngine;

public class CombatSystem : MonoBehaviour
{
    public Animator _anim;
    public ThirdPersonController movement;
    public PlayerAttributes playerattributes;

    private bool animplaying = false;


    IEnumerator lightattack()
    {
        animplaying = true;
        movement._canMove = false;
        Debug.Log("attacking");
        _anim.SetBool("lightattack", true);
        yield return new WaitForSecondsRealtime(0.650f);
        _anim.SetBool("lightattack", false);
        Debug.Log("stop attacking");
        movement._canMove = true;
        animplaying = false;
    }



    
    private void Update()
    {
        if (Input.GetButtonDown("Fire1") && playerattributes.hasWeaponEquiped && !animplaying && !_anim.GetBool("dodging"))
        {
            StartCoroutine(lightattack());
        }

        if (Input.GetKeyDown(KeyCode.Space) && playerattributes.currentStamina >= 25 && !animplaying && !_anim.GetBool("lightattack"))
        {
            animplaying = true;
            movement._canMove = true;
            _anim.SetBool("dodging", true);
            playerattributes.currentStamina -= 25;
            Debug.Log("dodging");
        }

        if (_anim.GetCurrentAnimatorStateInfo(0).IsName("dodge") && animplaying)
        {
            Debug.Log("stop dodging");
            _anim.SetBool("dodging", false);
            animplaying = false;
        }

    }
}
