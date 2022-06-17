using System;
using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using TMPro;
using UIScripts;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using static SkillTree;
using static PotionCooldown;

public class CombatSystem : MonoBehaviour {
    public Animator _anim;
    public ThirdPersonController playerMovement;
    public PlayerAttributes playerattributes;

    public GameObject potionUI;
    public int maxpotions;
    public int potions;
    public bool potionlootable = false;
    public GameObject currentPotion;
    public float regenerationTimer;
    public GameObject potioneffect;
    private bool canusepotion;
    public TextMeshProUGUI potionsUI;
    public bool invincible = false;
    
    public List<int> potionTickTimer = new List<int>();
    
    private void Start()
    {
        canusepotion = true;
        maxpotions = 3;
        refillPotions();
    }
    

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Potion"))
        {
            currentPotion = other.gameObject;
            potionUI.SetActive(true);
            potionlootable = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Potion"))
        {
            currentPotion = null;
            potionUI.SetActive(false);
            potionlootable = false;
        }
    }

    public void refillPotions()
    {
        potions = maxpotions;
        potionsUI.text = $"{potions}/{maxpotions}";
    }
    
    
    IEnumerator movementCooldown() {
        yield return new WaitForSecondsRealtime(0.650f);
        playerMovement._canMove = true;
    }
    
    public IEnumerator BecomeTemporarilyInvincible()
    {
        invincible = true;
        Debug.Log("Player turned invincible!");
        yield return new WaitForSeconds(1);
        invincible = false;
    }
    
    public IEnumerator regeneratingHealth()
    {
        if (skillTree.skillLevels[9] == 0) regenerationTimer = 0.4f;
        else regenerationTimer = 0.5f - skillTree.skillLevels[9] * 0.19f;
        while (potionTickTimer.Count > 0)
        {
            for (int i = 0; i < potionTickTimer.Count; i++)
            {
                potionTickTimer[i]--;
            }

            if (playerattributes.currentHealth < playerattributes.maxHealth) playerattributes.currentHealth += 0.20f;
            else potionTickTimer.Clear();
            potionTickTimer.RemoveAll(i => i == 0);
            yield return new WaitForSeconds(regenerationTimer);
        }
    }

    public void applypotion(int ticks)
    {
        if (potionTickTimer.Count <= 0)
        {
            potionTickTimer.Add(ticks);
            StartCoroutine(regeneratingHealth());
        }
        else potionTickTimer.Add(ticks);
    }
    
    public void PlayPotionEffect()
    {
        var newPotionEffect = Instantiate(potioneffect, transform.position + (Vector3.up * 0.35f), transform.rotation * Quaternion.Euler (-90f, 0f, 0f));
        newPotionEffect.transform.parent = gameObject.transform;
        Destroy(newPotionEffect, 5f);
    }
    
    public void LoseHealth(int amount)
    {
        if (invincible) return;

        playerattributes.currentHealth -= amount; //ARMOR DMG REDUCTION HERE

        if (playerattributes.currentHealth <= 0)
        {
            
        }
    }
    

    private void Update() 
    {
        if (Input.GetButtonDown("Fire1") && playerattributes.currentStamina >= 8 && playerattributes.hasWeaponEquiped && !_anim.GetCurrentAnimatorStateInfo(0).IsName("dodge") && !UiScreenManager._isOneIngameUiOpen) 
        {
            playerMovement._canMove = false;
            _anim.Play("lightattack");
            playerattributes.currentStamina -= 8;
            StartCoroutine(movementCooldown());
        }

        if (Input.GetKeyDown(KeyCode.E) && potionlootable && currentPotion != null)
        {
            Destroy(currentPotion);
            maxpotions++;
            potionsUI.text = $"{potions}/{maxpotions}";
            currentPotion = null;
            potionUI.SetActive(false);
            potionlootable = false;
        }

        if (Input.GetKeyDown(KeyCode.Space) && playerattributes.currentStamina >= 25 && !_anim.GetCurrentAnimatorStateInfo(0).IsName("lightattack") && !UiScreenManager._isOneIngameUiOpen) 
        {
            if (!invincible)
            {
                StartCoroutine(BecomeTemporarilyInvincible());
            }
            _anim.Play("dodge");
            playerattributes.currentStamina -= 20;
        }

        if (Input.GetKeyDown(KeyCode.G) && potions > 0 && !potioncooldown.isCooldown)
        {
            PlayPotionEffect();
            potions--;
            potionsUI.text = $"{potions}/{maxpotions}";
            applypotion(100 * (1 + skillTree.skillLevels[9]));
            potioncooldown.UsePotion(5);
        }

    }
}
