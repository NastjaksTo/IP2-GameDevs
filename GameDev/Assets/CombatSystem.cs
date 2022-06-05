using System;
using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using UnityEngine;

public class CombatSystem : MonoBehaviour {
    public Animator _anim;
    public ThirdPersonController playerMovement;
    public PlayerAttributes playerattributes;



    IEnumerator movementCooldown() {
        yield return new WaitForSecondsRealtime(0.650f);
        playerMovement._canMove = true;
    }

    private void Update() {
        if (Input.GetButtonDown("Fire1") && playerattributes.currentStamina >= 8 && playerattributes.hasWeaponEquiped && !_anim.GetCurrentAnimatorStateInfo(0).IsName("dodge") && !UiScreenManager._isOneIngameUiOpen) {
            playerMovement._canMove = false;
            _anim.Play("lightattack");
            playerattributes.currentStamina -= 8;
            StartCoroutine(movementCooldown());
        }


        if (Input.GetKeyDown(KeyCode.Space) && playerattributes.currentStamina >= 25 && !_anim.GetCurrentAnimatorStateInfo(0).IsName("lightattack") && !UiScreenManager._isOneIngameUiOpen) {
            _anim.Play("dodge");
            playerattributes.currentStamina -= 20;
        }


    }
}
